using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class detectClicks : MonoBehaviour, IPointerClickHandler
{
	static public Sprite img;//картинка для раскраски
	public static Sprite background;//задний фон

	public GameObject particleEffect;
	public static bool toClear = false;

	Color32[] originImgPixels;//пиксели оригинального изображения
	Color32[] imgPixels;//пиксели активного изображения
	int h, w;//висота и ширина изображения
	public static bool exit;
	bool[,] weWere;//масив по которому проверяем били ли ми в поточном пикселе
	public Texture2D pureImage;//чистой спрайт
	public static Color col;//цвет заднего фона
	public static bool wasClick;//можно ли сохранять игру
	public static Color color;//вибраной цвет
	public GameObject redCircle;//картинка с красним цветом
	public static bool letColoring;
	static Stack<GameObject> images;

	public GameObject[] ballons;

	public Texture2D originTexture;

	public static int number;

	bool compareColorWithWhite(Color a)//сравнение цвета с белим
	{
		return a.r >= Color.white.r - 0.6 && a.r <= Color.white.r + 0.6 && a.g >= Color.white.g - 0.6 && a.g <= Color.white.g + 0.6 && a.b >= Color.white.b - 0.6 && a.b <= Color.white.b + 0.6;
	}

	public Image imgOnGame;
	Image backgroundOnGame;
	public static List<GameObject> prefabs;
	public static bool isPainting;
	public static bool letShowPicture;

	public static bool showImage;
	public static bool wasClear;

	private void Awake()//при старте
	{
		wasClear = true;
		showImage = false;
		letShowPicture = true;
		images = new Stack<GameObject>();
		letColoring = true;
		if (isPainting)
		{
			wasClick = true;
			//получаем силки
			//background = gameObject.transform.parent.GetChild(0).gameObject.GetComponent<Image>();//на картинку для раскраски
			//img = GetComponent<Image>();//и задний фон

			GetComponent<SpriteRenderer>().sprite.texture.SetPixels(img.texture.GetPixels());
			//gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(img.texture, new Rect(0,0, img.texture.width, img.texture.height),new Vector2();
			//gameObject.transform.parent.GetChild(0).gameObject.GetComponent<Image>().sprite = background;
			//imgOnGame = GetComponent<Image>();
			//backgroundOnGame = gameObject.transform.parent.GetChild(0).gameObject.GetComponent<Image>();
			if (ContollActiveColor.active != null)//если ранее бил вибран цвет
				ContollActiveColor.active.GetComponent<Outline>().effectDistance = Vector2.zero;//удаляем тени длянего

			ContollActiveColor.active = redCircle;//присваиваем для активной картинки с изображением красний круг
			ContollActiveColor.active.GetComponent<Outline>().effectDistance = Vector2.one * 5;//создаем тени для него
			color = ContollActiveColor.active.GetComponent<Image>().color;//достаем с нее цвет и запоминаем его
			FreeDraw.Drawable.Pen_Colour = color;

			//wasClick = false;//запрещаем сохранять игру
			//считиваем оригинальное изображение 
			originImgPixels = pureImage.GetPixels32();

			h = pureImage.height;//запоминаем висоту 
			w = pureImage.width;//и ширину картинки которую будем раскрашивать
			return;
		}

		//получаем силки
		//background = gameObject.transform.parent.GetChild(0).gameObject.GetComponent<Image>();//на картинку для раскраски
		//img = GetComponent<Image>();//и задний фон

		GetComponent<Image>().sprite = img;
		gameObject.transform.parent.GetChild(0).gameObject.GetComponent<Image>().sprite = background;
		imgOnGame = GetComponent<Image>();
		backgroundOnGame = gameObject.transform.parent.GetChild(0).gameObject.GetComponent<Image>();
		if (ContollActiveColor.active != null)//если ранее бил вибран цвет
			ContollActiveColor.active.GetComponent<Outline>().effectDistance = Vector2.zero;//удаляем тени длянего

		ContollActiveColor.active = redCircle;//присваиваем для активной картинки с изображением красний круг
		ContollActiveColor.active.GetComponent<Outline>().effectDistance = Vector2.one * 5;//создаем тени для него
		color = ContollActiveColor.active.GetComponent<Image>().color;//достаем с нее цвет и запоминаем его
		h = imgOnGame.sprite.texture.height;//запоминаем висоту 
		w = imgOnGame.sprite.texture.width;//и ширину картинки которую будем раскрашивать
		gameObject.transform.parent.GetChild(0).gameObject.GetComponent<Image>().color = col;//устанавливаем цвет для заднего фона
		wasClick = false;//запрещаем сохранять игру
						 //считиваем оригинальное изображение 
		originImgPixels = Resources.Load<Sprite>("origine_pictures/" + imgOnGame.sprite.texture.name).texture.GetPixels32();
		int count = 0;
		foreach (Color pixel in imgOnGame.sprite.texture.GetPixels())
		{
			if (pixel == Color.white)
				++count;
			if (count > 200)
				break;
		}
		if ((count <= 200 && backgroundOnGame.color != Color.white && wasClear))
			wasClear = false;
	}

	public void OnPointerClick(PointerEventData eventData)//при клике на изображение
	{
		if (!letColoring)
			return;
		if (!letShowPicture)
			return;

		//particleEffect.GetComponent<ParticleSystem>().startColor = color;
		if (letColoring)
			Destroy(Instantiate(particleEffect, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity), 3);
		GetComponent<AudioSource>().Play();//проигриваем звуковой ефект
		imgPixels = imgOnGame.sprite.texture.GetPixels32();//считиваем пиксели активной картинки
		weWere = new bool[w, h];//виделяем память под маисв

		if (showImage)
        {
			instantiatePicture();
			return;
		}
			
		//узнаем координати пикселя на которий нажали
		Vector2 localCursor;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localCursor);
		Rect r = RectTransformUtility.PixelAdjustRect(GetComponent<RectTransform>(), GetComponent<Canvas>());
		Vector2 ll = new Vector2(localCursor.x - r.x, localCursor.y - r.y);
		int x, y;
		x = (int)(ll.x / r.width * w);
		y = (int)(ll.y / r.height * h);


		wasClick = true;//разрешаем сохранение игри
        if (img.texture.GetPixel(x, y).a == 0)//если пиксель прозрачний
        {
			//то меняем цвет заднего фона
            backgroundOnGame.color = color;
            //background.texture.Apply();
        }
        else//иначе делаем заливку области
            Floodfill(new Vector2Int(x, y));//заливка области
		int count = 0;
		foreach (Color pixel in imgOnGame.sprite.texture.GetPixels())
		{
			if (pixel == Color.white)
				++count;
			if (count > 200)
				break;
		}
		if (count <= 200 && backgroundOnGame.color != Color.white&& wasClear)
		{
			letShowPicture = false;
			letColoring = false;

			wasClear = false;
			StartCoroutine(wait());
			instantiateBallons();
		}
	}

	public static void instantiatePicture()
    {
		GameObject tmp = Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Count)], new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)));
		images.Push(tmp);
	}

	private void Floodfill(Vector2Int q)
	{
		Stack<Vector2Int> stack = new Stack<Vector2Int>();//создаем стек с координатами пикселей
		if (q.y < 0 || q.y > h - 1 || q.x < 0 || q.x > w - 1)//если зашли за предели картинки
			return;//виходим с функции

		stack.Push(q);//закидиваем в стек координату нажатого пикселя

		while (stack.Count > 0)//пока в стеке есть елементи
		{
			Vector2Int p = stack.Pop();//забираем со стека верхний елемент
			int x = p.x;
			int y = p.y;//присваиваем значения координатам
			if (y < 0 || y > h - 1 || x < 0 || x > w - 1)//если зашли за предели картинки то
				continue;//не делаем ничего

			if (!weWere[x, y] && compareColorWithWhite(originImgPixels[y * w + x]))//если ми еще не проверяли пиксель и он белий на оригинальной картинке
			{
				imgPixels[y * w + x] = color;//то закрашиваем пиксель

				weWere[x, y] = true;//запрминаем что етот пиксель закрашен
				//пробуем пойти вправо
				if (x < w - 2 && !weWere[x + 1, y] && compareColorWithWhite(originImgPixels[y * w + x + 1]))//если пиксель справа не закрашен и он белий на оригинальной картинке
					stack.Push(new Vector2Int(x + 1, y));//закидиваем новую координату в стек
				//пробуем пойти влево
				if (x > 0 && !weWere[x - 1, y] && compareColorWithWhite(originImgPixels[y * w + x - 1]))//если пиксель слева не закрашен и он белий на оригинальной картинке
					stack.Push(new Vector2Int(x - 1, y));//закидиваем новую координату в стек
				//пробуем пойти вниз
				if (y < h - 2 && !weWere[x, y + 1] && compareColorWithWhite(originImgPixels[(y + 1) * w + x]))//если пиксель снизу не закрашен и он белий на оригинальной картинке
					stack.Push(new Vector2Int(x, y + 1));//закидиваем новую координату в стек
				//пробуем пойти вверх
				if (y > 0 && !weWere[x, y - 1] && compareColorWithWhite(originImgPixels[(y - 1) * w + x]))//если пиксель внизу не закрашен и он белий на оригинальной картинке
					stack.Push(new Vector2Int(x, y - 1));//закидиваем новую координату в стек
			}
		}
		imgOnGame.sprite.texture.SetPixels32(imgPixels);//записиваем пиксели в картинку
		imgOnGame.sprite.texture.Apply();//и обновляем ее
		
	}

	IEnumerator wait()
    {
		exit = true;
		yield return new WaitForSeconds(8f);

		letColoring = true;
		letShowPicture = true;
	}

	void instantiateBallons()
    {
        for (int i = 0; i < 25; ++i)
            Destroy(Instantiate(ballons[UnityEngine.Random.Range(0, ballons.Length)],
                new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-15f, 2f)),
                Quaternion.identity),8);

    }

	public void Update()//каждий кадр
	{
		//if(Input.GetMouseButtonUp(0)&&isPainting&&!letColoring)
  //      {
		//	instantiatePicture();
		//	return;
		//}
		if (toClear)
			clear();
		//проверяем 
		if (exit&&wasClick)//не виходим ли ми в главное меню и можно ли сохранять игру
		{
			exit = false;
			if(isPainting)
            {
				PlayerPrefs.SetString("paint" + number.ToString(), Convert.ToBase64String(GetComponent<SpriteRenderer>().sprite.texture.EncodeToPNG()));
				//print(PlayerPrefs.HasKey("drawing" + number.ToString()));
			}
            else
            {
				//сохраняем строкой изображение
				PlayerPrefs.SetString(imgOnGame.sprite.name, Convert.ToBase64String(GetComponent<Image>().sprite.texture.EncodeToPNG()));
				//сохраняем строкой цвет заднего фона
				PlayerPrefs.SetString("background_color " + imgOnGame.sprite.name, ColorUtility.ToHtmlStringRGBA(transform.parent.GetChild(0).GetComponent<Image>().color));
			}
			PlayerPrefs.Save();
			images.Clear();
			//isPainting = false;
			//loadImg.nameImage = img.name;//перезагружать будем только одно изображение с заланим именем
			//loadImg.reload = true;//разрешаем перезагрузку картинки на главним меню
		}
	}

	void clear()
	{
		wasClear = true;
		toClear = false;
		wasClick = false;
		for (int i = images.Count; i > 0; --i)
			Destroy(images.Pop());
		if(isPainting)
        {
			//PlayerPrefs.DeleteKey("drawing"+number.ToString());

			Color[] whitePixels = new Color[GetComponent<SpriteRenderer>().sprite.texture.GetPixels().Length];
			for (int i = 0; i < whitePixels.Length; ++i)
				whitePixels[i] = Color.white;

			GetComponent<SpriteRenderer>().sprite.texture.SetPixels(whitePixels); //=Sprite.Create(Resources.Load<Sprite>("origine_pictures/" + img.sprite.texture.name).texture, new Rect(0,0,600,700),Vector2.zero);
			GetComponent<SpriteRenderer>().sprite.texture.Apply();
			PlayerPrefs.SetString("paint" + number.ToString(), Convert.ToBase64String(GetComponent<SpriteRenderer>().sprite.texture.EncodeToPNG()));
			return;
		}
		PlayerPrefs.DeleteKey(imgOnGame.sprite.texture.name);
		PlayerPrefs.DeleteKey("background_color " + imgOnGame.sprite.name);

		//GetComponent<Image>().sprite = Resources.Load<Sprite>("origine_pictures/" + imgOnGame.sprite.texture.name);

		GetComponent<Image>().sprite.texture.SetPixels32(originImgPixels); //=Sprite.Create(Resources.Load<Sprite>("origine_pictures/" + img.sprite.texture.name).texture, new Rect(0,0,600,700),Vector2.zero);
		GetComponent<Image>().sprite.texture.Apply();

		imgOnGame = GetComponent<Image>();
		gameObject.transform.parent.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
	}
}