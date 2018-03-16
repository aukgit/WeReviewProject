
var FILELIST = [];
var FILELOCATION = null;
var EXTENSION = null;

var path = require('path'), fs=require('fs');
function fromDir(startPath,filter){  

    if (!fs.existsSync(startPath)){
        console.log("no dir ",startPath);
        return;
    }
    var files=fs.readdirSync(startPath);
    
    for(var i=0;i<files.length;i++){       
         	FILELIST.push(files[i]);
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

function readConfig()
{
    var obj = require('./config.json');
    EXTENSION = obj.Extension;
    FILELOCATION = obj.FileLocation;  
    
}

readConfig();
fromDir(FILELOCATION,EXTENSION);
printHtml();


