mergeInto(LibraryManager.library, {

  GetLanguage: function (){
    if (ysdk !== null){
      var returnStr = ysdk.environment.i18n.lang;
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      return buffer;
    }
    else{
      return '';
    }
  },

  ReviewInternal: function()
  {
    Review();
  },
  
  PromptShowInternal: function()
  {
    PromptShow();
  },
  
  GetURLFromPage: function () {
        var returnStr = (window.location != window.parent.location) ? document.referrer : document.location.href;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
    
        return buffer;
    },
  
  OpenURL: function (url) {
    window.open(UTF8ToString(url), "_blank");
  
    //var a = document.createElement("a");
    //a.setAttribute("href", UTF8ToString(url));
    //a.setAttribute("target", "_blank");
    //a.click();
  },

  GameReadyAPI: function() {
    if (ysdk !== null && ysdk.features.LoadingAPI !== undefined && ysdk.features.LoadingAPI !== null) {
      ysdk.features.LoadingAPI.ready();
      //FullAdShow();
      console.log('Game Ready');
    }
    else{
      console.error('Failed - Game Ready');
    }
  }

});