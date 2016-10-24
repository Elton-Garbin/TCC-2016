<% Response.StatusCode = 403 %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="shortcut icon" type="image/png" href="http://localhost/StartIdea/favicon.ico" />
    <link href="http://localhost/StartIdea/Content/bootstrap.css" rel="stylesheet" />
    <title>Erro 403</title>
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="jumbotron text-center" style="margin-top: 7%;">
                    <h1 style="color: red;">403 Acesso negado</h1>
                    <br />
                    <p>O acesso a página solicitada foi negado por falta de permissões</p>
                    <br />
                    <button type="button" class="btn btn-large btn-info" onclick="window.history.back()">OK</button>
                </div>
                <br />
            </div>
        </div>
    </div>
</body>
</html>