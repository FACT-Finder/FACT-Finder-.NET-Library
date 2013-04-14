function FFSuggest() {
	
	var pRequest;
	var pLayer;
	var pDebug					= false;
	var pInstanceName			= '';
	var pSearchURL				= '';
	var pQueryParamName			= '';
	var pFormname 				= '';
	var pLayerName				= '';
	var pQueryInput;
	var pSuggest				= new Array();
	var pLastQuery;
	var pCurrentSelection		= 0;
	var submitted				= false;
	var pShowImages				= false;

	var pSuggestImageClass 		= 'suggestImage';
	var pSuggestQueryClass 		= 'suggestTextQuery';
	var pSuggestTypeClass 		= 'suggestTextType';
	var pSuggestAmountClass     = 'suggestTextAmount';
	var pSuggestQueryTypedClass = 'suggestTextQueryTyped';
	var pSuggestFooterClass     = 'suggestFooter';
	var pSuggestHeaderClass     = 'suggestHeader';
	var pSuggestRowClass	    = 'suggestRow';
	var pSuggestHighlightClass  = 'suggestHighlight';

	this.init = function(searchURL, formname, queryParamName, divLayername, instanceName, debugMode, channelParamName, channel, showImages) {
		pSearchURL			= searchURL;
		pFormname			= formname;
		pQueryParamName		= queryParamName;
		pChannelParamName	= channelParamName;
		pChannel			= channel;
		pLayerName			= divLayername;
		pInstanceName		= instanceName;
 		pDebug				= debugMode;
 		pShowImages			= showImages;
		if (pSearchURL == '') {		
			if (pDebug) alert('no searchurl defined');
			return null;
		} else if (pInstanceName == '') {
			if (pDebug) alert('no instancename defined');
			return null;
		} else if (pFormname == '') {
			if (pDebug) alert('no formname defined');
			return null;
		} else if (pQueryParamName == '') {
			if (pDebug) alert('no queryparamname defined');
			return null;
		} else if (pLayerName == '') {
			if (pDebug) alert('need a layer for output');
		}
		pQueryInput = document[pFormname][pQueryParamName];
		pQueryInput.onkeyup	= handleKeyPress;
		pQueryInput.onfocus	= showLayer;
		pQueryInput.onblur	= hideLayer;
		document[pFormname].onsubmit = handleSubmit;
	}
	
	function handleSubmit() {
		submitted = true;
		if (pSuggest[pCurrentSelection] != undefined) {
			document[pFormname][pQueryParamName].value = pSuggest[pCurrentSelection].split('###')[0];
			createQueryFromSuggestField();
		}
	}
	
	this.handleClick = function() {
		if (pSuggest[pCurrentSelection] != undefined) {
			document[pFormname][pQueryParamName].value = pSuggest[pCurrentSelection].split('###')[0];
			createQueryFromSuggestField();
			document[pFormname].submit();
		}
	}
	
	this.handleMouseOver = function(pos) {
		var tblCell = getTableCell(pos);
		unmarkAll();
		if (tblCell != null) {
			highlightSuggest(tblCell);
			pCurrentSelection = pos;
		}
	}
	
	this.handleMouseOut = function(pos) {
		var tblCell = getTableCell(pos);
		if (tblCell != null) {
			unmarkSuggest(tblCell);
			pCurrentSelection = -1
		}
	}
	
	function handleKeyPress(evt) {
		evt = (evt) ? evt : ((event) ? event : null);
		var keyCode = evt.keyCode;
		if (keyCode == 38) {
			moveSelection('up')
		} else if (keyCode == 27) {	
			hideLayer();
		} else if (keyCode == 40) {
			moveSelection('down');
		} else {
			if (pQueryInput.value == '') {
				hideLayer();
				if (pLayer != null){ pLayer.innerHTML = ''; }
				return null;
			}
			if (pLastQuery != pQueryInput.value){ startAjax(); }
			pLastQuery = pQueryInput.value;
		}
	}
	
	function moveSelection(direction) {
		var pos = pCurrentSelection;
		if (direction == 'up'){	pos--; }
		else{ 					pos += 1; }
		
		if (pos < 0) {
			unmarkAll();
			pQueryInput.focus();
			pCurrentSelection	= -1;
		} else {
			var tblCell = getTableCell(pos);
			if (tblCell != null) {
				unmarkAll();
				highlightSuggest(tblCell);
				pCurrentSelection = pos;
			}
		}
		
		var query = pQueryInput.value;
		pQueryInput.value = '';
		pQueryInput.focus();
		pQueryInput.value = query; 
	}
	
	function startAjax() {
		var query = pQueryInput.value;
		if(query.length > 50 ){ return; }
		
		var requestURL = pSearchURL +'?'+ pQueryParamName +'='+ encodeURIComponent(query) +'&'+ pChannelParamName +'='+ pChannel;
		 
		try {
			if( window.XMLHttpRequest ) {
				pRequest = new XMLHttpRequest();
			} else if( window.ActiveXObject ) {
				pRequest = new ActiveXObject( "Microsoft.XMLHTTP" );
			} else {
				if (pDebug) alert( 'no ajax connection' );
			}
			
			pLayer = document.getElementById(pLayerName);
			if (pLayer != null) {
				if (query != '') {

					pRequest.open( "GET", requestURL, true );
					pRequest.onreadystatechange = callbackAjax;
					pRequest.send( null );
				} else {
					hideLayer();
				}
			} else {
				if (pDebug) alert( 'no layer for output found' );
			}
		} catch( ex ) {
			hideLayer();
			if (ex == undefined) {
				if (pDebug) alert( 'Error: ' + ex.getmessage );
			} else {
				if (pDebug) alert( 'Error: ' + ex );
			}
		}
	}
	
	function hideLayer() {
		if (pLayer != null) {
			pLayer.style.display = 'none';
			fireSuggestLayerHidden();
		}
	}
	
	this.hideLayerOutsideCall = function() {
		if (pLayer != null) {
			pLayer.style.display = 'none';
			fireSuggestLayerHidden();
		}
	}
	
	function showLayer() {
		if (pLayer != null && pSuggest != null && pSuggest.length >= 1) {
			pLayer.style.display = 'block';
		}
	}
	
	function callbackAjax() {
		if (submitted == false) {
			if (pRequest.readyState == 4) {
				if (pRequest.status != 200) {
					hideLayer();
					if (pDebug) alert( 'Error (' + pRequest.status + '): ' + pRequest.statusText );
				} else {
					handleResponse(pRequest.responseText);
				}
			}
		}
  }

	// calls the callback for "outside" listeners if the callback is implemented
	function fireSuggestCompleted(suggestLayerIsVisible) {
		if (typeof(onSuggestCompleted) == 'function') {
			onSuggestCompleted(suggestLayerIsVisible);
		}
	}
	
	// calls the callback for "outside" listeners if the callback is implemented
	function fireSuggestLayerHidden() {
		if (typeof(onSuggestLayerHidden) == 'function') {
			onSuggestLayerHidden();
		}
	}

	function handleResponse(text) {
		var colSpan = 3;
		if(pShowImages){colSpan++;}
		
		pCurrentSelection = -1;
		pSuggest = new Array();
		pSuggest = text.split('\n');
		var outputText = '<table cellpadding="0" cellspacing="0" class="' + pLayerName + '" width="100%" border="0" onMouseDown="' + pInstanceName + '.handleClick();">';
		outputText += '<tr class="'+pSuggestHeaderClass+'" ><td nowrap="nowrap" colspan="'+colSpan+'">Vorschl&#228;ge zu Ihrer Suche...</td></tr>';
		
		var pNewSuggest = new Array();
		for (var i = 0; i < pSuggest.length; i++) {
			var firstChar = pSuggest[i].charCodeAt(0);
			
			if (firstChar != 13 && firstChar != 10 && pSuggest[i].length >= 1) {
				pNewSuggest.push(pSuggest[i]);
			}
		}
		pSuggest = pNewSuggest;
		var query = pQueryInput.value;
		
		for (var i = 0; i < pSuggest.length; i++) {
			pSuggestParts = new Array();
			pSuggestParts = pSuggest[i].split("###");
			
			outputText += '<tr id="' + pLayerName + '_' + i + '" class="'+pSuggestRowClass+'" onMouseOver="' + pInstanceName + '.handleMouseOver(' + i + ');" onMouseOut="' + pInstanceName + '.handleMouseOut(' + i + ');">';
			if(pShowImages){
				outputText +=	'<td nowrap="nowrap" class="'+ pSuggestImageClass +'"><img src="' + pSuggestParts[3] + '" alt=""/></td>';
			}
			outputText +=		'<td nowrap="nowrap" class="'+ pSuggestQueryClass +'">' + pSuggestParts[0].replace(new RegExp("("+query+")","ig"),'<span class="'+pSuggestQueryTypedClass+'">$1</span>') + '</td>'
								+'<td nowrap="nowrap" class="'+ pSuggestTypeClass +'">' + pSuggestParts[2] + '</td>'
								+'<td nowrap="nowrap" class="'+ pSuggestAmountClass +'">' + pSuggestParts[1] + '</td>'
						+'</tr>';
		}
		outputText += '<tr><td class="'+pSuggestFooterClass+'" colspan="'+colSpan+'">&nbsp;</td></tr></table>';
		if (pSuggest.length >= 1) {
			showLayer();
			pLayer.innerHTML = outputText;

			// calback for "outside" listeners
			fireSuggestCompleted(true);
		} else {
			hideLayer();
			pLayer.innerHTML = '';
			
			// calback for "outside" listeners
			fireSuggestCompleted(false);
		}
		
	}
	
	function highlightSuggest(tblCell) {
		tblCell.className = pSuggestHighlightClass; 
	}
	
	function unmarkSuggest(tblCell) {
		tblCell.className = pSuggestRowClass; 
	}
	
	function unmarkAll() {
		var tblCell;
		for (var i = 0; i < pSuggest.length; i++) {
			tblCell = getTableCell(i);
			if (tblCell != null) {
				unmarkSuggest(tblCell);
			}
		}
	}
	
	function getTableCell(pos) {
		var tblCell;
		tblCell = document.getElementById(pLayerName + '_' + pos);
		return tblCell;
	}
	
	//creates a hidden input field to pass, so we know this query was chosen from suggest
	function createQueryFromSuggestField(){
		var element = document.createElement('input');
		element.name = 'queryFromSuggest';
		element.type = 'hidden';
		element.value = 'true';
		document[pFormname].appendChild(element);
	}
}