function ExecuteScript(strId)
{
  switch (strId)
  {
      case "5zhHM5xjo0G":
        Script1();
        break;
      case "6nUgrGrjnIm":
        Script2();
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
  var player = GetPlayer();

var myName  = lmsAPI.GetStudentName();

var array  = myName.split(',');
var personsName = array[1] + '  ' + array[0];

player.SetVar("usrName", personsName);
}

