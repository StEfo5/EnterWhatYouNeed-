using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class main : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject go_input, go_alert, go_alertText, go_score, go_virus, 
        go_certificate, go_wallpaper, go_result, go_gameOver, go_canvas, go_minigame;
    public GameObject[] go_outputs, go_tasks;
    Text t_input, t_alert, t_score, t_virus, t_result;
    Text[] t_tasks = new Text[3];
    Image i_wallpaper;
    public Sprite[] spr_wallpapers;
    public AudioClip[] sounds;
    int score = 100, 
        virus = 0, 
        taskNumber = 0, 
        numberOfTasks = 3, 
        numberOfWallpapers = 6,
        searchEnginePenalty = 5,
        outputPenalty = 15,
        maxNumberOfViruses = 5;
    bool isAlerting, isExplanation;
    string ans,
        wrongAnswer = "Неверно!",
        sampleScene = "SampleScene";
    string[] seAnswers = {
        	"маркетинг",
        	"война и мир л.н.толстой ext:pdf",
        	"скачать антивирус"
        },
        taskAlerts = {
            "Всегда выбирайте проверенные сайты, например Википедию и обращайте внимание на начало ссылки сайта а именно на “https://” - безопасно “http://” - не всегда.", 
            "Иногда найти файл удобней, чем искать по названию, для этого в Яндексе после названия напишите “mime:pdf”, или в Google “ext:pdf”.",
            "Вы прошли игру! Поделитесь с друзьями вашим достижением! (ИКОНКА Whatsapp, Instagramm, VK)" 
        },
        tasks = {
            "1. Узнай значение слова маркетинг\n(Для этого напишите в ввод слово “маркетинг”)",
            "2. Найди файл “Война и мир Л. Н. Толстой”\n(Для нахождения непосредственно файла, допишите в конце “ext:pdf”)",
            "3. Скачай антивирус\n(Напишите в поисковик слова “Скачать антивирус”)"
        },
        unintendedMoves = {
            "Эх, ты... Иди по сценарию!!!"
        };
    void Start()
    {
        t_input = go_input.GetComponent<Text>();
        t_alert = go_alertText.GetComponent<Text>();
        t_score = go_score.GetComponent<Text>();
        t_virus = go_virus.GetComponent<Text>();
        t_result = go_result.GetComponent<Text>();
        for(int i = 0; i < 3; i++)t_tasks[i] = go_tasks[i].GetComponent<Text>();
        i_wallpaper = go_wallpaper.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("return") && !isAlerting){
        	Search();
        }
        else if(Input.GetKeyDown("return") && isAlerting){
            HideAlert();
        }
    }

    //ввод запроса в поиковик
    public void Search(){
        if(t_input.text.Length == 0){
            EnterWrong();
        } //если введенный текст пуст
        else{
        	ans = null;
            //преобразуем запрос в преемлемый вид
        	if(!(t_input.text[0] == ' '))ans += Char.ToLower(t_input.text[0]);
        	for(int i = 1; i < t_input.text.Length; i++) {
        		if(!((t_input.text[i-1] == '.' || t_input.text[i-1] == ' ') && t_input.text[i] == ' '))
        		ans += Char.ToLower(t_input.text[i]);
        	}
        	if(ans[ans.Length-1]==' ')ans = ans.Substring(0, ans.Length-1);
            //если ввод неверный
        	if(ans != seAnswers[taskNumber]){
                EnterWrong();
        	}
            //иначе
        	else{
        		go_outputs[taskNumber].SetActive(true);
                GetComponent<AudioSource>().PlayOneShot(sounds[0]);
        	}
        }
    }

    //верный выбор
    public void ChooseTrue(int a_taskNumber){
        //если задание не последнее
        if(a_taskNumber!=numberOfTasks){
            if(!isAlerting)GetComponent<AudioSource>().PlayOneShot(sounds[2]);
            ShowAlert(taskAlerts[a_taskNumber-1]);
            taskNumber = a_taskNumber;
            t_tasks[taskNumber].text = tasks[taskNumber];
        }
        //иначе
        else{
            GetComponent<AudioSource>().PlayOneShot(sounds[4]);
            t_result.text = "Очки: " + score.ToString() + "\n" + "Вирусы: " + virus.ToString();
            go_certificate.SetActive(true);
        }
        isExplanation = true;
    }

    //неверный выбор
    public void ChooseFalse(){
        //если сейчас не выходит сообщение
        if(!isAlerting){
            GetComponent<AudioSource>().PlayOneShot(sounds[3]);
            LowerScore(outputPenalty);
            AddVirus();
            t_alert.text = wrongAnswer;
            ShowAlert(wrongAnswer);
        }
    }

    //показать сообщение
    void ShowAlert(string alertText){
        t_alert.text = alertText;
        go_alert.SetActive(true);
        isAlerting = true;
    }

    //скрыть сообщение
    public void HideAlert(){
        go_alert.SetActive(false);
        isAlerting = false;
        //если это было обьяснение
        if(isExplanation){
            go_canvas.SetActive(false);
            go_minigame.SetActive(true);
            FindObjectOfType<playerController>().StartGame();
            isExplanation = false;
        }
    }

    //понизить очки
    public void LowerScore(int subtr){
        score -= subtr;
        t_score.text = "очки: " + score.ToString();
        if(score<=0){
            LoseGame();
        }
    }

    //добавить вирус
    public void AddVirus(){
        virus++;
        t_virus.text = "вирусы: " + virus.ToString();
        //поменять фон если вирусов меньше количество фонов
        if(virus<numberOfWallpapers)
            i_wallpaper.sprite = spr_wallpapers[virus];
        //если количество вирусов больше максимально допустимого
        if(virus>maxNumberOfViruses)
            LoseGame();
    }

    void EnterWrong(){
        LowerScore(searchEnginePenalty);
        if(!isAlerting && score!=0)GetComponent<AudioSource>().PlayOneShot(sounds[1]);
        go_outputs[taskNumber].SetActive(false);
        ShowAlert(unintendedMoves[0]);
    }

    void LoseGame() {
        GetComponent<AudioSource>().PlayOneShot(sounds[5]);
        go_gameOver.SetActive(true);
    }

    public void StartOver() {
        SceneManager.LoadScene(sampleScene);
    }
}
