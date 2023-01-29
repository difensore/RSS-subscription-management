# RSS-subscription-management  
Use this endpoints for:  
/Account/register (POST, parameters "Email" and "Password", password must contain at least 1 capital letter, 1 number and 1 special symbol) to sign up  
/Account/login (parameters "Email" and "Password") to sign in  
/Account/logout to logout  
/RSSFeeds/allnews to get all news  
/RSSFeeds/rssbydate (parameter "date" (DD/MM/YYYY)) to get unread news by date  
/RSSFeeds/rssmark (POST, parameter "url" to news) to set news as readed  
/RSSFeeds/newrss (POST, parametr "url" to Rss feed) to save Rss Feed to database  
/RSSFeeds/allfeeds to get Rss feeds titles
