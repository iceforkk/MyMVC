$(document).ready(function  () {
	$(".comment_title ul li").click(function(){
	var index = $(this).index();
//		console.log(index);
		$(".comment_title ul li").removeClass("ct_lion");
		$(".comment_title ul li").eq(index).addClass("ct_lion");
		$(".px_dz").hide();
		$(".px_dz").eq(index).show();
	});
	
	$(".comment_hf").click(function  () {
		$(".comment_area").show();
		$(".comment_hf_praise").hide();
	});
	$(".comment_area p").click(function(){
		$(".comment_area").hide();
		$(".comment_hf_praise").show();
	});
 


});



	






































