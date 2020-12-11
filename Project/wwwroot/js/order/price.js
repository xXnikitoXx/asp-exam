const price = $("tbody td:nth-child(4)");
const midPrice = $("tbody td:nth-child(6)");
const discount = $("tbody td:nth-child(7)");
const finalPrice = $("tbody td:nth-child(8)");
const couponList = $("code");

const DefaultPrice = () => Number(price.text().slice(0, -2));
const Coupons = () => couponList.text().split(", ");

const GetDiscount

const UpdatePrice = () => {

};

$("#amount").change(() => {

});