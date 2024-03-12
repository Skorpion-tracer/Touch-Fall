var isRewarded = false;

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
        console.log("Результат оценки" + feedbackSent);
        if (feedbackSent) {
          // Если оценка поставлена отправляем в Unity false и скрываем кнопку
          myGameInstance.SendMessage('Yandex', 'CanRateGameCallback', 0);
        }
        else {
          // Иначе отправляем в Unity true и оставляем кнопку видимой
          myGameInstance.SendMessage('Yandex', 'CanRateGameCallback', 1);
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
    if (player) {
        player.getData().then(_data => {
        const myJSON = JSON.stringify(_data);
        myGameInstance.SendMessage('Yandex', 'LoadData', myJSON);
        console.log(myJSON);
      });
    } else {
      console.log("player don`t init");
    }
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

  ShowAdv: function() {
      ysdk.adv.showFullscreenAdv({
      callbacks: {
          onClose: function(wasShown) {
            myGameInstance.SendMessage('Yandex', 'StartGame');
          },
          onError: function(error) {
            myGameInstance.SendMessage('Yandex', 'StartGame');
          }
      }
    });
  },

  ShowAdvStartGame: function() {
      ysdk.adv.showFullscreenAdv({
      callbacks: {
          onClose: function(wasShown) {
            myGameInstance.SendMessage('Root', 'CallbackAfterShowAdv');
            console.log('Adv close');
          },
          onError: function(error) {
            myGameInstance.SendMessage('Root', 'CallbackAfterShowAdv');
            console.log('Adv error');
          }
      }
    });
  },

  ShowAdvRewarded: function() {
    ysdk.adv.showRewardedVideo({
        callbacks: {
          onOpen: () => {
            console.log('Video ad open.');
          },
          onRewarded: () => {
            console.log('Rewarded!');
            isRewarded = true;
          },
          onClose: () => {
            if (isRewarded) {
              myGameInstance.SendMessage('Yandex', 'AfterRewarded');
            }
            else {
              console.log('Close before watching the ad');
              myGameInstance.SendMessage('Yandex', 'UnLockBtns');
            }
          }, 
          onError: (e) => {
            console.log('Error while open video ad:');
            myGameInstance.SendMessage('Yandex', 'UnLockBtns');
          }
      }
    });
  },

});