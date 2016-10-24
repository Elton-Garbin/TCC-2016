<link href="http://localhost/StartIdea/Content/bootstrap.css" rel="stylesheet" />

<body>
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="jumbotron text-center" style="margin-top: 7%;">
                    <h1 style="color: red;">Página não encontrada</h1>
                    <br />
                    <p>A página requisitada não existe</p>
                    <br />
                    <a href="#" class="btn btn-large btn-info" name="btnVoltar" onclick="goBack()">OK</a>
                </div>
                <br />
            </div>
        </div>
    </div>
</body>

<script type="text/javascript">
    function goBack() {
        window.history.back();
    }
</script>