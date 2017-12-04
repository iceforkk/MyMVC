//(function ($) {
//$.fn.slideDown = function (duration) {    
//  // get old position to restore it then
//  var position = this.css('position');
//  
//  // show element if it is hidden (it is needed if display is none)
//  this.show();
//  
//  // place it so it displays as usually but hidden
//  this.css({
//    position: 'absolute',
//    visibility: 'hidden'
//  });
// 
//  // get naturally height
//  var height = this.height();
//  
//  // set initial css for animation
//  this.css({
//    position: position,
//    visibility: 'visible',
//    overflow: 'hidden',
//    height: 0
//  });
// 
//  // animate to gotten height
//  this.animate({
//    height: height
//  }, duration);
//};
//})(Zepto);

$(document).ready(function(){
	//搜索内容的切换
	$(".search_bar1_3").click(function(){
		$(".search_bar1").hide();
		$(".search_bar1_inner").show();
		$(".search_bar2").show();
	});
	$(".search_bar1_inner1").click(function(){
		$(".search_bar1").show();
		$(".search_bar1_inner").hide();
		$(".search_bar2").hide();
	});
	
});
$(document).ready(function(){
	var length =$(".wrap ul li").length;
	for(var i=0;i<length;i++){
		var text = $(".wrap ul li").eq(i).find(".courseli_right_bottom2").html();
		var text1 = text.trim();
		if(text1=="已选"){
		$(".wrap ul li").eq(i).find(".courseli_right_bottom2").css({"background":"#00b4ff"})
	}else{
		$(".wrap ul li").eq(i).find(".courseli_right_bottom2").css({"background":"#bfbfbf"})
	}
	}

});
$(document).ready(function(){
	document.addEventListener('touchmove', function (event) {
event.preventDefault();
}, false);
	//siderBar的展开关闭
	$(".sideBar_list1_title2.open").click(function(){
		$(this).parent().find(".sideBar_list1_title2.open").hide();
		$(this).parent().find(".sideBar_list1_title2.close").show();
		$(this).parent().find(".sideBar_list1_title2 img").attr("src","../images/down1x.png");
		$(this).parent().parent().find(".sideBar_list2").hide();
	});
	$(".sideBar_list1_title2.close").click(function(){
		$(this).parent().find(".sideBar_list1_title2.open").show();
		$(this).parent().find(".sideBar_list1_title2.close").hide();
		$(this).parent().find(".sideBar_list1_title2 img").attr("src","../images/up1x.png");
		$(this).parent().parent().find(".sideBar_list2").show();
	});
	
	$(".sideBar_list2_title2.open").click(function(){
		$(this).parent().find(".sideBar_list2_title2.open").hide();
		$(this).parent().find(".sideBar_list2_title2.close").show();
		$(this).parent().find(".sideBar_list2_title2").attr("src","../images/close1x.png");
		$(this).parent().parent().find("ul").hide();
	});
	$(".sideBar_list2_title2.close").click(function(){
		$(this).parent().find(".sideBar_list2_title2.open").show();
		$(this).parent().find(".sideBar_list2_title2.close").hide();
		$(this).parent().find(".sideBar_list2_title2").attr("src","../images/open1x.png");
		$(this).parent().parent().find("ul").show();	
	});
	//勾选
	$(".sideBar_list2 ul li").click(function(){
		$(".sideBar_list2 ul li").find(".sideBar_list2_lispan2").css({"background":""});
		$(this).find(".sideBar_list2_lispan2").css({"background":"url(../images/dh1x.png) no-repeat"});
	});
})

$(document).ready(function(){
	$(".sb1_click").click(function(){
		$(".sb1_click").hide();
		$(".sb1_clicked").show();
		$(".sideBar").animate({"left":"0%"},300)
	});
	$(".sb1_clicked").click(function(){
		$(".sb1_clicked").hide();
		$(".sb1_click").show();
		$(".sideBar").animate({"left":"-50%"},300)
	})
})




//$(document).ready(function(){
//	//判断展开图标的有无
//	var length1 = $(".sideBar_list1").length;
//	console.log(length1);
//	for(var i=0;i<length1;i++){
//		var length2 = $(".sideBar_list1").eq(i).find(".sideBar_list2").length;
//		var length3 = $(".sideBar_list1").eq(i).find(".sideBar_list2 ul li").length;
//		console.log(length3);
//		if(length2 == 0){
//			$(".sideBar_list1").eq(i).find(".sideBar_list1_title").find(".sideBar_list1_title2").hide();
//		};
//		if (length3 == 0) {
//			$(".sideBar_list1").eq(i).find(".sideBar_list2 ").find(".sideBar_list2_title2").hide();
//		}
//	}
//	
//})























































































