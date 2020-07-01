mergeInto(LibraryManager.library, {

  OpenBrowserTabJS: function (url) {
    console.log("OpenBrowserTabJS");
    //window.alert(url);
    window.open(Pointer_stringify(url));
  },
  

  });