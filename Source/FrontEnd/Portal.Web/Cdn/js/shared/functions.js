// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

function progressBar() {
	var progressValue = 100; 
	var criticalProgressValue = 900; 
	var maxProgressValue = 1000; 
		$( "#progressbar" ).progressbar({
		  max: maxProgressValue,
		  value: progressValue
		});	
		if( $('#progressbar').attr('aria-valuenow')<criticalProgressValue ) {
			$('#progressbar').parents('.login-progress-used').find('.login-progress-text').html( progressValue + ' MB of 1 GB used');
		}		
		if( $('#progressbar').attr('aria-valuenow')>=criticalProgressValue ) {
			$('#progressbar').parents('.login-progress-used').addClass('progressbar-full');
			$('#progressbar').parents('.login-progress-used').find('.login-progress-text').html( progressValue + ' MB of 1 GB used');
		}
}

function pluso() {
	(function() {
  if (window.pluso)if (typeof window.pluso.start == "function") return;
  if (window.ifpluso==undefined) { window.ifpluso = 1;
    var d = document, s = d.createElement('script'), g = 'getElementsByTagName';
    s.type = 'text/javascript'; s.charset='UTF-8'; s.async = true;
    s.src = ('https:' == window.location.protocol ? 'https' : 'http')  + '://share.pluso.ru/pluso-like.js';
    var h=d[g]('body')[0];
    h.appendChild(s);
  }})();
}

/* inputFocus */
var parentArray = ['.search-form, .holder-input'];
//inputFocus(parentArray);
function inputFocus(parent){
	var parentSize = parent.length;
	var n = 0;
	for ( n; n < parentSize; n++ ) {
		var obj = parent[n];
		var input = $(obj).find(':text, textarea');
		if ( $(input).length ) {
			$(obj).each(function(){
				$(this).on('click', function(e){
					var thisObj = $(this);
					input = thisObj.find(':text, textarea, :password');
					$(this).addClass('focus');
					$(this).parents('.search-form').addClass('focus');
					$(input).trigger('focus');
					$(input).on('blur', function(){
						thisObj.removeClass('focus');
						$(this).parents('.search-form').removeClass('focus');
					});
					e.preventDefault();
				});
			});
		}
	}
}
/* inputFocus end */

/* tabs */
function tabs(){
	var tabWrap = $('.tabs-wrap');
	tabWrap.each(function(){
		var thisTabWrap = $(this);
		var tabControl = thisTabWrap.find('.tab-controls');
		var controlItem = tabControl.find('a');
		var itemControl = tabControl.find('li.active').index();
		var tabsWrap = thisTabWrap.find('.tabs');
		var tab = tabsWrap.find('.tab');
		//tab.eq(itemControl).fadeIn(0).siblings().fadeOut(0);
		controlItem.on('click', function(e){
			var $this = $(this);
			var index = $this.parents('li').index();
			if ( $this.parent('li').hasClass('active') ) {
				$this.parents('li').removeClass('active');
				tab.fadeOut(300);
			} else {
				$this.parents('li').addClass('active').siblings().removeClass('active');
				tab.fadeOut(0).eq(index).fadeIn(300);
			}			
			e.preventDefault();
		});
	});
}
/* tabs end */

/* dropdown */
function dropdown() {
	if ( $('.registration-field').length ) {
		$('.registration-field .holder-country-text').on('click',function() {
			$(document).unbind('click.myEvent');
			$('.select-country').fadeOut();
			var cur = $(this);
			$('.registration-field .holder-country-text').not(cur).removeClass('active');
			$('.edit-profile').removeClass('active');
			if ( !cur.hasClass('active') ) {
				cur.addClass('active');
				cur.parents('.edit-profile').addClass('active');
				var drop = cur.parent().find('.select-country').fadeIn();
				var yourClick = true;
			
				$(document).bind('click.myEvent', function (e) {
					if (!yourClick  && $(e.target).closest(drop).length == 0 || $(e.target).closest('.select-country-item').length == 1 ) {
						drop.fadeOut();
						$('.registration-field .selected-country').removeClass('active');
						$('.edit-profile').removeClass('active');
						$(document).unbind('click.myEvent');
						$('.edit-profile').removeClass('active');
			$('.holder-country-text').removeClass('active');
					}
					yourClick  = false;
				});
			} else {
				cur.removeClass('active');
				cur.parents('.edit-profile').removeClass('active');
				$('.select-country').fadeOut();
			}
		});
		$('.select-country-item').on('click', function(e) {
			var cur = $(this);
			var attrLink = cur.data('value');
			var textLink = cur.text();
			cur.parents('.edit-profile').find('.selected-country').text(textLink);
			cur.parents('.edit-profile').find('input[type="hidden"]').val(attrLink);
			$('.edit-profile').removeClass('active');
			$('.holder-country-text').removeClass('active');				
		});		
		$('.russian-federation').on('click', function() {	
			$(this).parent('.edit-profile').find('.holder-input').addClass('red');			
			$('.holder-input.ein').fadeOut();	
			$('.c-address').addClass('red');
			$('.edit-profile').removeClass('notBlock');
		});
		$('.united-states').on('click', function() {
			if( ! $('.edit-profile').hasClass('notBlock') ) {
				$('.edit-profile').addClass('notBlock');
				$('.holder-input.ein').fadeIn();
			}		
			$('.c-address').addClass('red');				
		});
	}
}
/* dropdown end */

/* placeholder */
function placeholderEdit() {
	if ($.browser.webkit || $.browser.mozilla) {
		$('input,textarea').on('click', function(){
		var placeholder = $(this).attr('placeholder');
		$(this).removeAttr('placeholder');
						
		$(this).on('blur', function(){
		$(this).attr('placeholder',placeholder);
		  });
	   });
	}
}
/* placeholder end */

/* checkbox */
function checkbox(){
	if ( $(':checkbox').length && $.fn.checkbox ){
			$(':checkbox').checkbox({cls:'checkbox'});
		};
}


/* form subscriber*/
function formSubscriber() {
	$('.regular-item .btn-silver').click(function(){
		$('.regular-form').fadeIn(700);
		$('.subscriptions-list').fadeOut(100);
		return false;
	});
	$('.partner-item .btn-green').click(function(){
		$('.partner-form').fadeIn(700);
		$('.subscriptions-list').fadeOut(100);
		return false;
	});
	$('.premium-item .btn-red').click(function(){
		$('.premium-form').fadeIn(700);
		$('.subscriptions-list').fadeOut(0);
		return false;
	});
	$('.regular-form input:checkbox').on('change', function(){
		if ( $(this).is(':checked') ) {
			$('.regular-form').find(':submit').removeAttr('disabled');
			$('.subscriptions-form-bottom').removeClass('disabled');	
		} else {
			$('.regular-form').find(':submit').attr('disabled', 'disabled');
			$('.subscriptions-form-bottom').addClass('disabled');	
		}
	});
	$('.partner-form input:checkbox').on('change', function(){
		if ( $(this).is(':checked') ) {
			$('.partner-form').find(':submit').removeAttr('disabled');
			$('.subscriptions-form-bottom').removeClass('disabled');	
		} else {
			$('.partner-form').find(':submit').attr('disabled', 'disabled');
			$('.subscriptions-form-bottom').addClass('disabled');
		}
	});
	$('.premium-form input:checkbox').on('change', function(){
		if ( $(this).is(':checked') ) {
			$('.premium-form').find(':submit').removeAttr('disabled');
			$('.subscriptions-form-bottom').removeClass('disabled');	
		} else {
			$('.premium-form').find(':submit').attr('disabled', 'disabled');
			$('.subscriptions-form-bottom').addClass('disabled');
		}
	});
	
	$('.subscriptions-form-bottom .btn-silver').click(function(){
		 setTimeout(function(){
			$('.subscriptions-list').fadeIn(700);
			$('.subscriptions-form').fadeOut(0);
			$('.subscriptions-form-bottom').addClass('disabled');
			$('.subscriptions-form').find(':submit').attr('disabled', 'disabled');
		 },200);		
	});
}
/* form end */
$(document).ready(function(){
	checkbox();
	formSubscriber();
	progressBar() ;
	inputFocus(parentArray);
	tabs();
	placeholderEdit();
	
		var winW = $(this).width();
		if ( winW <= 1440 && winW >= 1145 ) {
			var colW = parseInt($('.column-holder').css('width'));
			var constanta = 219 + 20;
			var margLeft = colW - constanta;
			$('.list-tags-holder').css('margin-right', -margLeft+'px');
		} else {
			$('.list-tags-holder').css('margin-right', '-50px');
		}
	
	$(window).resize(function(){
		var winW = $(this).width();
		if ( winW <= 1440 && winW >= 1145 ) {
			var colW = parseInt($('.column-holder').css('width'));
			var constanta = 219 + 20;
			var margLeft = colW - constanta;
			$('.list-tags-holder').css('margin-right', -margLeft+'px');
		} else {
			$('.list-tags-holder').css('margin-right', '-50px');
		}
	});
	//dropdown();
});