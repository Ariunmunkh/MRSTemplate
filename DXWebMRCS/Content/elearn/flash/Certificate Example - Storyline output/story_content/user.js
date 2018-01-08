function ExecuteScript(strId)
{
  switch (strId)
  {
      case "6XFqaehpobU":
        Script1();
        break;
      case "6H3LkLn7zrf":
        Script2();
        break;
      case "5sQVI95DAPw":
        Script3();
        break;
      case "5XQp8PxeGH4":
        Script4();
        break;
      case "6BcC9eYckxx":
        Script5();
        break;
  }
}

function Script1()
{
  var currentTime = new Date()
var month = currentTime.getMonth() + 1
var day = currentTime.getDate()
var year = currentTime.getFullYear()
var dateString=month + "/" + day + "/" + year

var player = GetPlayer();
player.SetVar("SystemDate",dateString);
}

function Script2()
{
  function getQueryVal(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split('&');
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split('=');
        if (decodeURIComponent(pair[0]) == variable) {
            return decodeURIComponent(pair[1]);
        }
    }
}
var player=GetPlayer();
//storyline variables userName and userID must exist for this to work
player.SetVar("newName", getQueryVal("usrName"));
player.SetVar("LessonName",getQueryVal("LessonName"));
}

function Script3()
{
  if (document.location.href.indexOf('html5') < 0) {
GetPlayer().printSlide()
} else {
if(!window.hasPrintStyle){
window.hasPrintStyle = true;
var css = '@media print {body * {visibility: hidden;}#slidecontainer, #slidecontainer * {visibility: visible;}#slidecontainer {position: absolute;left: 0;top: 0; }#slideframe {overflow: visible;}}',
head = document.head || document.getElementsByTagName('head')[0],
style = document.createElement('style');
style.type = 'text/css';
if (style.styleSheet){
style.styleSheet.cssText = css;
} else {
style.appendChild(document.createTextNode(css));
}
head.appendChild(style);
}
var whereNow = $("#slidecontainer").offset();
$("#slidecontainer").offset({top:0,left:0});
window.print();
$("#slidecontainer").offset(whereNow);
}
}

function Script4()
{
  var player = GetPlayer();

var myName  = lmsAPI.GetStudentName();

var array  = myName.split(',');
var newName = array[1] + '  ' + array[0];

player.SetVar("newName", newName);

}

function Script5()
{
  var player = GetPlayer();

var myName  = lmsAPI.GetStudentName();

var array  = myName.split(',');
var newName = array[1] + '  ' + array[0];

player.SetVar("newName", newName);

}

