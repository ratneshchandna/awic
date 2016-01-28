<% Response.StatusCode = 404 %>

<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <meta name="description" content="Error">
        <title>Page Not Found</title>
        <link href='https://fonts.googleapis.com/css?family=PT+Sans:400,700' rel='stylesheet' type='text/css'>
        <style>
            html, body {
                height: 100%;
            }
            .text-center {
                text-align: center;
            }
            .text-danger {
                color: #eb0000;
            }
            .h1-div {
                margin: 20% 0 10% 0;
            }
            .h1-div > h1 {
                font-family: 'PT Sans', 'sans-serif';
                font-size: 28pt;
                text-transform: none;
            }
            .h2-div > h2 {
                font-family: 'PT Sans', 'sans-serif';
                font-size: 24pt;
                text-transform: none;
            }
        </style>
    </head>
    <body>
        <div class="h1-div">
            <h1 class="text-danger text-center">Sorry, we couldn't find what you were looking for ... </h1>
        </div>
        <div class="h2-div">
            <h2 class="text-center">Try working your way from the <a href="/">home page</a>, maybe?</h2>
        </div>
    </body>
</html>