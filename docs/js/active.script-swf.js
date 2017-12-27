var dialogOn = false;
//подготовка среды
function prepare_environment() {
    //диалоговый модуль
    document.body.innerHTML += "<div id='dialog' class='dialog' style='margin-left:-35px;'>" +
        "<div class='label' onclick='toggleDialog()'>Ask questions</div>" +
        "<div class='header'><p class='text-center'>Log:<p></div>" +
        "<div class='history' id='history'></div>" +
        "<div class='question'><input id='Qdialog' class='input' placeholder='Write a question'/><br>" +
        "<button class='btn' onclick='ask(\"Qdialog\")'>Go</button>"
    "</div>" +
    "</div>";
    //крупный план изображений
    document.body.innerHTML += "<div id='imgalert'  style='display:none'>" +
        "<div class='bg' onclick='hide(\"imgalert\")'>&nbsp;</div>" +
        "<img id='img_in_alert' src='' />" +
        "</div>";
    //поле с распознаванием речи
    // Задаем API-ключ (подробнее см. Глобальные настройки API).
    window.ya.speechkit.settings.apikey = '5c6d6536-b453-4589-9bc7-f16c7a795106';

    // Добавление элемента управления "Поле для голосового ввода".
    var textline = new ya.speechkit.Textline('Qdialog', {
        onInputFinished: function(text) {
            // Финальный текст.
            ask('Qdialog');
        }
    });
}

//ДИАЛОГ
//показ-скрытие диалогового модуля
function toggleDialog() {
    var timer;
    //закрытие
    if (dialogOn) {
        $("#dialog").animate({ "margin-left": "-35px" }, 1000, function() {});
        dialogOn = false;
        timer = setInterval(alert_over_time, timeout);
    }
    //открытие
    else {
        $("#dialog").animate({ "margin-left": "-300px" }, 1000, function() {});
        dialogOn = true;
        clearInterval(timer);
    }
}


//база знаний
var knowledge = [
    [
        "примесная проводимость полупроводников",
        "- это",
        "электрическая проводимость, обусловленная наличием в полупроводнике донорных или акцепторных примесей"
    ],
    [
        "собственная проводимость полупроводников",
        "- это",
        "электрическая проводимость, при которой ток создается равным количеством электронов и «дырок»"
    ],
    [
        "основные носители заряда",
        "- это",
        "дырки полупроводника"
    ],
    [
        "неосновные носители заряда",
        "- это",
        "электроны полупроводника"
    ],
    [
        "p-n-переход",
        "- это",
        "область соприкосновения двух полупроводников с разными типами проводимости"
    ],
    [
        "электронно-дырочный переход",
        "- это",
        "область соприкосновения двух полупроводников с разными типами проводимости"
    ],
    [
        "энергетическая диаграмма",
        "выглядит",
        "следующим образом: <img src='img/diagramm.png'>"
    ],
    [
        "контактная разность потенциалов",
        "- это",
        "разность потенциалов, возникающая при соприкосновении двух различных твердых проводников, имеющих одинаковую температуру"
    ],
    [
        "дрейфовый ток",
        "- это",
        "ток, возникающий за счет приложенного электрического поля"
    ],
    [
        "диффузионный ток",
        "- это",
        "ток, возникающий из-за неравномерной концентрации носителей заряда"
    ],
    [
        "вольтамперная характеристика",
        "выглядит",
        "следующим образом: <img src='img/volt-amper-diagramm.png'>"
    ],
    [
        "вольт-амперная характеристика",
        "выглядит",
        "следующим образом: <img src='img/volt-amper-diagramm.png'>"
    ],
    [
        "",
        "",
        ""
    ],
    [
        "",
        "",
        ""
    ],
    [
        "",
        "",
        ""
    ],

];

//поиск и вывод ответа и вопроса
function ask(questionInput) {
    var question = document.getElementById(questionInput).value.trim();
    //выдвижение диалогового модуля
    $("#dialog").animate({ "margin-left": "-300px" }, 1000, function() {});
    dialogOn = true;
    //вывод вопроса
    //document.getElementById("history").innerHTML+="<div class='question'>"+question+"</div>";
    var newDiv = document.createElement("div");
    newDiv.className = 'question';
    newDiv.innerHTML = question;
    document.getElementById("history").appendChild(newDiv);
    //поиск и вывод ответа
    //document.getElementById("history").innerHTML+="<div class='answer'>"+getAnswer(question)+"</div>";
    //создаем блок <div>
    newDiv = document.createElement("div");
    //задаем класс оформления созданного блока
    newDiv.className = 'answer';
    //получаем ответ на вопрос и наполняем им созданный блок
    newDiv.innerHTML = getAnswer(question);
    //ОЗВУЧКА - СИНТЕЗ РЕЧИ
    //флаг, нужна ли озвучка (не нужна, если есть анимация)
    var needSound = true;
    //проходим по элементам HTML-кода ответа
    for (var i = 0; i < newDiv.childNodes.length; i++) {
        //если находим элемент <embed>
        if (newDiv.childNodes[i].tagName == "EMBED") {
            //alert("EMBED detected.");
            //сбрасываем флаг и выходим из цикла
            needSound = false;
            break;
        }
    }
    //если флаг не был сброшен
    if (needSound) {
        //добавляем в ответ тег аудио, ссылающийся на звук от синтезатора речи яндекса
        //в обращении к яндексу tts.voicetech.yandex.net указывается:
        // - формат звука: format=wav
        // - язык озвучиваемого текста: lang=ru-RU
        // - ключ, полученный при регистрации в личном кабинете для SpeechKit Cloud API: key=4a4d3a13-d206-45fc-b8fb-e5a562c9f587
        // - озвучиваемый текст, который берется из сгенерированного ответа: text="+newDiv.innerText+"
        //alert("Incoming transmission.");
        newDiv.innerHTML += "<audio controls='true' autoplay='true' src='http://tts.voicetech.yandex.net/generate?format=wav&lang=ru-RU&key=4a4d3a13-d206-45fc-b8fb-e5a562c9f587&text=" + (newDiv.innerText || newDiv.textContent) + "'></audio>";
    }
    document.getElementById("history").appendChild(newDiv);
    //запуск звука
    if (newDiv.lastChild.tagName == "AUDIO") {
        newDiv.lastChild.play();
    }
    //прокрутка истории в самый низ
    document.getElementById("history").scrollTop = document.getElementById("history").scrollHeight;
    //очистка текстового поля для ввода вопроса
    document.getElementById(questionInput).value = "";
}

//псевдоокончания сказуемых (глаголов, кратких причастий и прилагательных )
var endings = [
    ["ет", "(ет|ут|ют)"],
    ["ут", "(ет|ут|ют)"],
    ["ют", "(ет|ут|ют)"],
    ["ыл", "(ал|ул|ыл)"], //1 спряжение
    ["ит", "(ит|ат|ят)"],
    ["ат", "(ит|ат|ят)"],
    ["ят", "(ит|ат|ят)"], //2 спряжение
    ["ется", "(ет|ут|ют)ся"],
    ["утся", "(ет|ут|ют)ся"],
    ["ются", "(ет|ут|ют)ся"], //1 спряжение, возвратные
    ["ится", "(ит|ат|ят)ся"],
    ["атся", "(ит|ат|ят)ся"],
    ["ятся", "(ит|ат|ят)ся"], //2 спряжение, возвратные
    ["ен", "ен"],
    ["ена", "ена"],
    ["ено", "ено"],
    ["ены", "ены"], //краткие прилагательные
    ["ан", "ан"],
    ["ана", "ана"],
    ["ано", "ано"],
    ["аны", "аны"], //краткие прилагательные
    ["жен", "жен"],
    ["жна", "жна"],
    ["жно", "жно"],
    ["жны", "жны"], //краткие прилагательные
    ["такое", "- это"]
]; //для вопроса "что такое А?" ответ - "А - это ..."
//черный список слов, распознаваемых как сказуемые по ошибке
var blacklist = ["замена", "замены", "атрибут", "маршрут", "член", "нет"];
//функция определения сказуемых по соответствующим псевдоокончаниям
function getEnding(word) {
    //проверка по черному списку
    if (blacklist.indexOf(word) !== -1) return -1;
    //перебор псевдоокончаний
    for (var j = 0; j < endings.length; j++) {
        //alert(word.substring(word.length-endings[j][0].length)+"-"+endings[j][0]);
        //проверка, оканчивается ли i-ое слово на j-ое псевдоокончание
        if (word.substring(word.length - endings[j][0].length) == endings[j][0]) {
            return j; //возврат номера псевдоокончания
        }
    }
    return -1; //если совпадений нет - возврат -1
}

//функция, которая делает первую букву маленькой
function small1(str) {
    return str.substring(0, 1).toLowerCase() + str.substring(1);
}
//функция, которая делает первую букву большой
function big1(str) {
    return str.substring(0, 1).toUpperCase() + str.substring(1);
}

//главная функция, обрабатывающая запросы клиентов
function getAnswer(question) {
    //знаки препинания
    var separators = "'\",.!?()[]\\/";
    //получение текста из параметра запроса
    var txt = small1(question);
    //добавление пробелов перед знаками препинания
    for (var i = 0; i < separators.length; i++)
        txt = txt.replace(separators[i], " " + separators[i]);
    //массив слов и знаков препинания
    var words = txt.split(' ');
    //флаг, найден ли ответ
    var result = false;
    //формируемый функцией ответ на вопрос
    var answer = "";
    //перебор слов
    for (var i = 0; i < words.length; i++) {
        //alert(words[i]);
        //поиск номера псевдоокончания 
        var ending = getEnding(words[i]);

        //если псевдоокончание найдено – это сказуемое, подлежащее в вопросе после него
        if (ending >= 0) {
            //замена псевдоокончания на набор возможных окончаний
            words[i] = words[i].substring(0, words[i].length -
                endings[ending][0].length) + endings[ending][1];
            //создание регулярного выражения для поиска по сказуемому из вопроса
            var predicate = new RegExp(words[i]);
            //для кратких прилагательных захватываем следующее слово
            if (endings[ending][0] == endings[ending][1]) {
                predicate = new RegExp(words[i] + " " + words[i + 1]);
                i++;
            }
            //создание регулярного выражения для поиска по подлежащему из вопроса
            var subject_array = words.slice(i + 1);
            //из слов подлежащего выбрасываем короткие предлоги (периметр у квадрата = периметр квадрата)
            for (var j = 0; j < subject_array.length; j++) {
                if (subject_array[j].length < 3) {
                    subject_array = subject_array.splice(j);
                    j--;
                }
            }
            var subject_string = subject_array.join(".*");
            //только если в послежащем больше трех символов
            if (subject_string.length > 3) {
                var subject = new RegExp(".*" +
                    subject_string +
                    ".*");
                //поиск совпадений с шаблонами среди связей семантической сети
                for (var j = 0; j < knowledge.length; j++) {
                    if (predicate.test(knowledge[j][1]) &&
                        (subject.test(knowledge[j][0]) ||
                            subject.test(knowledge[j][2]))) {
                        //создание простого предложения из семантической связи
                        answer += big1(knowledge[j][0] + " " +
                            knowledge[j][1] + " " + knowledge[j][2] + ". ");
                        result = true;
                    }
                }
                //если совпадений с двумя шаблонами нет,
                if (result == false) {
                    //поиск совпадений только с шаблоном подлежащего
                    for (var j = 0; j < knowledge.length; j++) {
                        if ((subject.test(knowledge[j][0]) ||
                                subject.test(knowledge[j][2]))) {
                            //создание простого предложения из семантической связи
                            answer += big1(knowledge[j][0] + " " +
                                knowledge[j][1] + " " + knowledge[j][2] + ". ");
                            result = true;
                        }
                    }
                }
            }
        }
    }
    //если ответа нет
    if (!result)
        answer = "Ответ не найден. <br/>";
    //если ответ есть - добавляем увеличение картинок
    else
        answer = answer.replace("<img ", "<img onclick='zoom(this.src)'");
    return answer;
}

function zoom(src) {
    document.getElementById("img_in_alert").src = src;
    $("#imgalert").css({ "display": "block" });
}

function hide(id) {
    document.getElementById(id).setAttribute("style", "display:none");
}