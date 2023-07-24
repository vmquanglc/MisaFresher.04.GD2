// globalExtensions.js
String.prototype.format = function () {
  var formattedString = this;
  for (var i = 0; i < arguments.length; i++) {
    var pattern = new RegExp("\\{" + i + "\\}", "g");
    formattedString = formattedString.replace(pattern, arguments[i]);
  }
  return formattedString;
};
