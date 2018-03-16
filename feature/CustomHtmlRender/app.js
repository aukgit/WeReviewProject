
var FILELIST = [];
var EXTENSION = '.markdown';
var path = require('path'), fs=require('fs');
function fromDir(startPath,filter){  

    if (!fs.existsSync(startPath)){
        console.log("no dir ",startPath);
        return;
    }
    var files=fs.readdirSync(startPath);
    
    for(var i=0;i<files.length;i++){
       // var filename=path.join(startPath,files[i]);
       // console.log(files[i]);
       // var stat = fs.lstatSync(filename);
       //  if (stat.isDirectory()){
       //     fromDir(filename,filter); //recurse
       //  }
       //else if (filename.indexOf(filter)>=0) {         
         	FILELIST.push(files[i]);
           // console.log(filename);         
       // }; 
    };
    	
};

function printHtml ()
{
    for(var i=0;i<FILELIST.length;i++){
	var showdown  = require('showdown'),
    converter = new showdown.Converter(),
    text      = FILELIST[i],
    html      = converter.makeHtml(text);
    console.log(html);
    }
}

fromDir('./files',EXTENSION);
printHtml();


