mergeInto(LibraryManager.library, {

  MakePostRequest: function (url, data) {
    let request = new XMLHttpRequest();
    request.open("POST", UTF8ToString(url));
    request.send(UTF8ToString(data)); 
	
    request.onload = function() {
      if (request.status != 200) {
        unityInstance.SendMessage('MetodicCanvas', 'OnRequestNetError');
      } 
      else {
        //var bufferSize = lengthBytesUTF8(returnStr) + 1;
        //var buffer = _malloc(bufferSize);
        //stringToUTF8(returnStr, buffer, bufferSize);
        unityInstance.SendMessage('MetodicCanvas', 'OnRequestResponse', request.response);
      };
    };
    request.onerror = function(request) {
      unityInstance.SendMessage('MetodicCanvas', 'OnRequestNetError');
    };

    return request;
  }

});