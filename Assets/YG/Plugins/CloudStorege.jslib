mergeInto(LibraryManager.library, {

  InitCloudStorage: function()
  {
    var returnStr = cloudSaves;
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },  

  SaveData: function (jsonData, flush)
  {
    SaveCloud(UTF8ToString(jsonData), flush);
  },
  
  LoadData: function (sendback) 
  {
    LoadCloud(sendback);
  }
  
});