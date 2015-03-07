function clickProduct(query, artId, artPos, artOrigPos, pageNum, artSimi, sessionId, artTitle, pageSize, origPageSize, channel, eventName){
    debug      = false;
    request    = null;
    requestUrl = 'tracking.cshtml';
    requestUrl += '?query=' + query;
    requestUrl += '&id=' + artId;
    requestUrl += '&pos=' + artPos;
    requestUrl += '&origPos=' + artOrigPos;
    requestUrl += '&page=' + pageNum;
    requestUrl += '&simi=' + artSimi;
    requestUrl += '&sid=' + sessionId;
    requestUrl += '&title=' + artTitle;
    requestUrl += '&pageSize=' + pageSize;
    requestUrl += '&origPageSize=' + origPageSize;
    requestUrl += '&channel=' + channel;
    requestUrl += '&event=' + eventName;

    try {
        if( window.XMLHttpRequest ) {
            request = new XMLHttpRequest();
        } else if( window.ActiveXObject ) {
            request = new ActiveXObject( "Microsoft.XMLHTTP" );
        } else {
            if (debug) alert( 'no ajax connection' );
        }

        if (request != null) {
            request.open( "GET", requestUrl, false );
            request.send( null );
        }
    } catch( ex ) {
        if (ex != undefined) {
            if (debug) alert( 'Error: ' + ex.getmessage );
        } else {
            if (debug) alert( 'Error: ' + ex );
        }
    }
}