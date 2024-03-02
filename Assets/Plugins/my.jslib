mergeInto(LibraryManager.library, {

  /*Hello: function () {
    window.alert("Hello, world!");
    console.log("Hello, world!");
  },*/

  CanRateGame: function () {
    console.log("Call ability rate game");
    ysdk.feedback.canReview()
    .then(({ value, reason }) => {
      if (value) {
        myGameInstance.SendMessage('Yandex', 'CanRateGameCallback', 1);
      }
      else {
        myGameInstance.SendMessage('Yandex', 'CanRateGameCallback', 0);
      }
      
      if (value) {
        console.log("Можем запрашивать оценку игры!!!");
      }
      else {
         console.log("Не возможно запросить оценку игры " + reason);
      }
    });
  },

  RateGame: function () {
   ysdk.feedback.canReview()
   .then(({ value, reason }) => {
    if (value) {
      ysdk.feedback.requestReview()
      .then(({ feedbackSent }) => {
        console.log(feedbackSent);
        if (feedbackSent) {
          myGameInstance.SendMessage('Yandex', 'CanRateGameCallback', 1);
        }
        else {
          myGameInstance.SendMessage('Yandex', 'CanRateGameCallback', 0);
        }
      });
    } else {
      console.log("Не возможно запросить оценку игры " + reason);
    }
  });
 },

  SaveExtern: function(data) {
    var dataString = UTF8ToString(data);
    var myobj = JSON.parse(dataString);
    player.setData(myobj);
  },

  LoadExtern: function() {
    console.log("Loading data player");
      player.getData().then(_data => {
        const myJSON = JSON.stringify(_data);
        myGameInstance.SendMessage('Yandex', 'LoadData', myJSON);
        console.log(myJSON);
      });
  },

  SetToLeaderBoard: function(value) {
    ysdk.getLeaderboards()
        .then(lb => {
          lb.setLeaderboardScore('Scores', value);
    });
  },

  GetLang: function() {
    var lang = ysdk.environment.i18n.lang;
    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

});