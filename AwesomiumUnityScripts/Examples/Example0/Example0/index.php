<!DOCTYPE html>
<html lang="en">
	<head>
		<link href="http://fonts.googleapis.com/css?family=Poiret+One|Crete+Round" rel="stylesheet" type="text/css">
		<link href="css/style0.css" rel="stylesheet" type="text/css">
		<script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>		
		<script>
			function UnityExists()
			{
				return (typeof window["Unity"] !== "undefined");
			}
		
			$(document).ready(function()
			{
				/*var MainMenuItems = $(".MainMenuItems > li");
				MainMenuItems.bind("mouseenter", function()
				{
					$(this).animate({marginLeft:'30px'}, {duration:50, queue:false});
				}).bind("mouseleave", function()
				{
					$(this).animate({marginLeft:'10px'}, {duration:50, queue:false});
				});*/
				
				$("#PlayButton").click(function()
				{
					if (UnityExists()) Unity.PlayGame();
				});
				
				$("#OptionsButton").click(function()
				{
					if (UnityExists()) Unity.GoToOptions();
				});
				
				$("#QuitButton").click(function()
				{
					if (UnityExists()) Unity.QuitGame();
				});
			});
		</script>
	</head>
	<body>
		<ul class="MainMenuItems">
			<li id="PlayButton">Play</li>
			<li id="OptionsButton">Options</li>
			<li id="QuitButton">Quit</li>
		</ul>
	</body>
</html>