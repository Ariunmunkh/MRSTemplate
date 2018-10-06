function ExecuteScript(strId)
{
  switch (strId)
  {
      case "6RyqHhpDBdp":
        Script1();
        break;
      case "6EbZx0MoRlx":
        Script2();
        break;
  }
}

function Script1()
{
  var player = GetPlayer();
var currentTime = new Date()
var month = currentTime.getMonth() + 1
var day = currentTime.getDate()
var year = currentTime.getFullYear()
var dateString=month + "/" + day + "/" + year

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
player.SetVar("SystemDate",dateString);
player.SetVar("username", getQueryVal("username"));
player.SetVar("LessonName",getQueryVal("lessonname"));
player.SetVar("lessonid",getQueryVal("lessonid"));
player.SetVar("userid",getQueryVal("userid"));

}

function Script2()
{
  var player = GetPlayer();
var urldate = player.GetVar("SystemDate");
var urlscore = player.GetVar("JavaResults");
var userid = player.GetVar("userid");
var lessonid = player.GetVar("lessonid");

 var xhr;
 if (window.XMLHttpRequest) xhr = new XMLHttpRequest();  
 else xhr = new ActiveXObject("Microsoft.XMLHTTP"); 
 xhr.onreadystatechange = function () 
 {
  if (xhr.readyState == 4 && xhr.status == 200) 
   {
     var serverResponse = JSON.parse(xhr.responseText);     
   }
 }
 var url = 'https://localhost:44300/api/eService/PosteService?score='+urlscore+'&date='+urldate+'&userid='+userid+'&lessonid='+lessonid;
 xhr.open("POST", url, true);
 xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
 xhr.send();
}

