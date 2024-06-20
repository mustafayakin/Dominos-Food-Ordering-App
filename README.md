# Dominos Food Ordering App

Restoran Programı; **9 adet** stored procedure, **3 adet** fonksiyon, **8 adet** trigger, **16 adet** Table'sinden oluşuyor.
**Üye girişi** ve **çalışan girişi** olmak üzere iki adet girişimiz bulunmakta. Üyeler için restorandan sipariş verip, kendi bilgilerini düzenleyebileceği ayrıca siparişi tamamlandıktan sonra sipariş hakkında yorum yapabileceği ekranlarımız bulunuyor. Başlangıça üye kayıtlı değilse *Kayıt Ol* ekranıyla üye kaydı yapılabilir. Burda kullanıcı adının başka üye tarafından kullanılıp kullanılmadığına dikkat ediliyor. Sipariş verirken her kullanıcının belirli bir seviyesi var bu seviyelerin de belirli **discount** oranları var buna göre siparişlerde kullanıcıya indirim sağlanıyor.

Admin panelimizde;

 - üye ekleme, üye silme, üye güncelleme, üye arama
 - yemek ekleme, yemek silme, yemek güncelleme
 - sipariş silme, sipariş güncelleme, sipariş arama
 - yorum işlemleri, yorum güncelleme

gibi menustrip’imiz bulunuyor. 

Kısaca anlatmak gerekirse: sipariş geldiğinde admin panelinde sipariş geldiğine dair bir bildirim bulunuyor, çalışan siparişi içeriğini ve adresini görüp siparişi tamamlayabiliyor veya iade edebiliyor. Yine diğer tüm özellikler çalışabiliyor. Ayrıca tüm ekranlara filtreleme özelliği de koyarak istenilen özellikteki View’e ulaşabiliyoruz. Genel çizgide programım bunları yapıyor. Arayüzüne ve siparişi alırken yemekleri array olarak tutmasına çok uğraştım çünkü bir veriyi array olarak almak çok zorladı. *Burası kısa tanıtım olduğu için uzatmıyorum, Bu raporun sonuna tüm eklediğim özelliklerin açıklamalarını ve görsellerini koyacağım.*

# İş Kurallarım

## Temel İş Kurallarım

 - Bir ürünün en fazla bir tane kategorisi olabilir. 
 - Birden fazla ürün aynı kategoride olabilir 
 - Her şehrin bir veya birden çok İlçesi olabilir.

## Üyelerin İş Kuralları
 - Her üye birden çok sipariş verebilir bir siparişi en çok bir üye verebilir
 - Her seviyenin bir çok üyesi olabilir. Bir üyenin birden çok seviyesi olamaz.
 - Her seviyenin bir discount bedeli vardır. Bir çok discount bir seviyeye verilemez.
 - Her sipariş iade edilebilir. bir sipariş birden fazla iade edilemez.
 - Her üyenin bir şehri ve ilçesi olabilir, bir şehir veya ilçe bir çok üyeye verilebilir.
 - Her üyenin bir kullanıcıadı olur, bir kullanıcıadı yalnızca bir kişiye ait olur.
## Siparişlerin İş Kuralları
 - Bir siparişi bir üye verebilir. bir üye birden çok sipariş verebilir.
 - Bir siparişte birden çok yemek olabilir. bir çok yemek bir siparişe verilebilir.
 - Bir siparişte bir ödeme biçimi olabilir, bir ödeme biçimi bir çok siparişe verilebilir.
 - Bir çalışan çok siparişi alabilir, bir sipariş en fazla bir çalışan tarafından alınabilir.
## Yorumların İş Kuralları
 - Bir üye birden çok yorum atabilir, bir yorum en çok bir üyeye ait olabilir
 - Sipariş Tamamlanmadan bir üye siparişe yorum yapamaz
 - Yorum bir çalışan tarafından cevaplanır, bir cevap en çok bir yoruma karşılık verilir
## Çalışanın İş Kuralları
 - Bir çalışanın bir yetkisi olabilir, bir yetki birden çok çalışanaverilebilir.
 - Bir siparişi bir çalışan alabilir, bir çalışan birden çok sipariş alabilir.

## Yemeklerin İş Kuralları
 - Bir yemeğin bir kategorisi olur, bir kategori bir çok yemeğe verilebilir.

# Varlık Bağıntı Modelim
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/varlikbaginti.PNG" height= 700>

# İlişkisel Şema Gösterimi
Çalışan Listesi:

    calisan_list(ID: smallint, ad: character varying(20), soyad:    character varying(20), kullaniciad: character varying(20), sifre:    character varying(20), yetki_id: smallint)
Çalışan Yetki Adları:

    calisan_yetki_adlari(yetki_id: smallint, yetki_adi: character varying(20))
İlçeler(Türkiyede Bulunan Tüm İlçeler:

    ilceler(id: smallint il_id: smallint, ilcead: character varying(30))
Şehirler(Türkiyede Bulunan Tüm Şehirler);

    sehirler(id: smallint, plaka_kodu: smallint, sehirad: character varying(30))
Kullanıcı Seviyelerinin İsimleri:

    seviyeadlari(seviyeid: smallint, seviyeadi: character varying(50))
Seviyelerin Discount Oranları:

    seviyediscount(id: smallint, seviyedisc: integer, seviyeid: smallint)
Tüm Seviyeli Üyelerin Bir Listesi:

    seviyeli_uyeler(id: smallint, kullanici_id: smallint, kullaniciadi: character varying(20),seviyeid: smallint)
Siparişlerin Listesi:

    siparis_list(id: smallint, secilenyemek: integer[], kullanicis: smallint, toplam: double precision, odemebicimi: smallint, tamamlanma: smallint, siparisialan: smallint)
Trigger Test için Oluşturulmuş Sipariş Tamamlandı mı?:

    siparis_tamamlandimi(id: smallint, durum: character varying(20))
Tüm Üyeler:

    uye_list(id: smallint, ad: character varying(15), soyad: character varying(15), sehirid: smallint,ilceid: smallint, 
    kullaniciad: character varying(15), sifre: character varying(25), adres: character varying(100), seviyeid: smallint)
Yemeklerin Kategorileri:

    yemek_kategori(id: smallint, yemek_id: smallint, kategori_id: smallint)
Yemeklerin Listesi:

    yemek_list(id: smallint, yemekad: character varying(50), yemek_kat: smallint, tutar: double precision)
Trigger test için yemek fiyatlarında oluşacak değişiklik:

    yemekfiyatdegisikligi(kayitno: integer, urunno: smallint, eskifiyat: real, yenifiyat: real,
    değişiklik_tarih: timestamp without time zone)
Yorumların cevaplanıp cevaplanmadığı?:

    yorum_durumlari(id: smallint, durum: character varying(20))
Tüm Yorumlar:

    yorumlar(id: smallint, yorum: character varying(100), yildiz: smallint, sipariş_durum: smallint, yorum_durum: smallint, cevap: character varying(100), kullanici_id: smallint)

## Stored Procedure
Birkaç stored proceduremi açıklamam gerekirse:
Çalışan ekle ve üye ekle prosedürlerim basitçe kullanıcıdan verileri alıp call sorgusuyla üye_list table’sine ve calisan_list table’sine insert komutu yolluyor.

    CREATE OR REPLACE PROCEDURE public.calisan_ekle(
    IN p1 text,
    IN p2 text,
    IN p3 text,
    IN p4 text,
    IN p5 integer)
    LANGUAGE 'sql'
    AS $BODY$
    insert into calisan_list (ad,soyad,kullaniciad,sifre,yetki_id) values (p1,p2,p3,p4,p5);
    $BODY$;
    ALTER PROCEDURE public.calisan_ekle(text, text, text, text, integer)
    OWNER TO postgres;
    CREATE OR REPLACE PROCEDURE public.uye_ekle(
    IN p1 text,
    IN p2 text,
    IN p3 integer,
    IN p4 integer,
    IN p5 text,
    IN p6 text,
    IN p7 text,
    IN p8 integer)
    LANGUAGE 'sql'
    AS $BODY$
    insert into uye_list (ad,soyad,sehirid,ilceid,kullaniciad,sifre,adres,seviyeid) values (p1,p2,p3,p4,p5,p6,p7,p8);
    $BODY$;
    ALTER PROCEDURE public.uye_ekle(text, text, integer, integer, text, text, text, integer)
    OWNER TO postgres;
Yemek Ekleme Prosedürüm:

    CREATE OR REPLACE PROCEDURE public.yemek_ekle(
    IN p1 text,
    IN p2 integer,
    IN p3 double precision)
    LANGUAGE 'sql'
    AS $BODY$
    insert into yemek_list (yemekad,yemek_kat,tutar) values (p1,p2,p3);
    $BODY$;
    ALTER PROCEDURE public.yemek_ekle(text, integer, double precision)
    OWNER TO postgres;
Yemek fiyatı güncellenmek istendiğinde çağırılan prosedürüm, İlgili ID'deki tutarı set ediyor:

    CREATE OR REPLACE PROCEDURE public.yemek_fiyat_guncelle(
    IN p1 integer,
    IN p2 integer)
    LANGUAGE 'sql'
    AS $BODY$
    update yemek_list SET tutar = p2 WHERE id = p1;
    $BODY$;
    ALTER PROCEDURE public.yemek_fiyat_guncelle(integer, integer)
    OWNER TO postgres;
    CREATE OR REPLACE PROCEDURE public.yemek_kategori_guncelle(
    IN p1 integer,
    IN p2 integer)
    LANGUAGE 'sql'
    AS $BODY$
    update yemek_list SET yemek_kat = p2 WHERE id = p1;
    $BODY$;
    ALTER PROCEDURE public.yemek_kategori_guncelle(integer, integer)
    OWNER TO postgres;
## Functions
İstenilen yorumu getiren sonuç olarak table döndüren bir fonksiyonum. İstenilen yorumu, yorum durumuna göre getiriyor.

    CREATE OR REPLACE FUNCTION public.getistenilenyorum(
    pr1 smallint)
    RETURNS TABLE(yorumid smallint, yorumu character varying, yorumdurummu smallint)
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000
    AS $BODY$
    Begin
    Return Query
    Select
    id,
    yorum,
    yorum_durum
    From
    yorumlar
    Where
    yorum_durum = pr1;
    End;
    $BODY$;;
İndirimsiz fiyatı hesaplayan fonksiyonum. Fonksiyona indirimli fiyatı girdikten sonra:
**tutar =100*tutar/100-discount**  ile indirimsiz tutarı buluyor.

    -- FUNCTION: public.indirimsizfiyat(double precision, integer)
    -- DROP FUNCTION IF EXISTS public.indirimsizfiyat(double precision, integer);
    CREATE OR REPLACE FUNCTION public.indirimsizfiyat(
    tutar double precision,
    discount integer)
    RETURNS double precision
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    AS $BODY$
    begin
    tutar = (100*tutar) / (100-discount);
    return tutar;
    end;
    $BODY$;
## Triggers
Deleteüye triggerim. Kısaca uye_list’ten bir üye silindiğinde seviyeli_uyeler table’sinden ilgili üye kaldırılıyor.

    CREATE OR REPLACE FUNCTION public.deleteuye()
    RETURNS trigger
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE NOT LEAKPROOF
    AS $BODY$
    BEGIN
    DELETE FROM seviyeli_uyeler
    WHERE seviyeli_uyeler.kullanici_id = OLD.id;
    RETURN NEW;
    END
    $BODY$;
-

    CREATE TRIGGER deleteuyetrigger
    AFTER DELETE
    ON public.uye_list
    FOR EACH ROW
    EXECUTE FUNCTION public.deleteuye();
# Program Ekran Görüntülerim
## Üye Ekran Görselleri

<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/1.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/2.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/3.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/4.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/5.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/6.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/7.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/8.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/9.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/10.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/11.PNG" height= 600>

Genel anlamıyla üye ekranım bu şekilde ekleme silme güncelleme arama işlemleri ve diğer işlemleri admin panelinden yapıyoruz. Kısa tanıtımda değinmediğim discount özelliği kullanıcıların belli bir seviyeleri var ve bu seviyeler belli oranda siparişte discount sağlıyor.

Ayrıca görselde bahsettiğim gibi siparişleri array integer içine atmak çok zorladı muhtemelen c#’da yeterli yetkinliğe sebep olmamadan dolayıdır. ayrıca burada görsele almadığım başka özelliklerde var bunlar;

 - Eğer kullanıcı sepetinde bir şey yokken göndere basarsa sepetinizde
   ürün yok hatası
 - Bilgilerim sayfasında şifreleri aynı girmezse şifreleriniz uyuşmuyor
   hatası
 - Yorum gönderdikten sonra aynı siparişe bir daha yorum göndermek
   isterse daha önce bu siparişe yorum yaptınız hatası
 - Sepetinde ürün yokken çıkar butonuna basarsa çıkarılacak ürün yok
   hatası
 - Bilgilerim kısmında herhangi bir textbox’ı boş bırakırsa bir textbox
   boş hatası
 - Üye Giriş ekranında k.adı ile şifre uyuşmazsa uyarı vermesi
 - Yine calisan_list, uye_list’in bir kalıtımı olduğu için adminler üye
   girişi yapmak isterse adminlerin girişi yasaktır hatası bunu yetki_id ile  kontrol ediyorum. Adminler üye girişi yapamaz çalışan girişi yapmalıdır.
 - Diğer tüm sayfalarda eğer bilgi yokken bilgi getire basılırsa bilgi
   yok hataları hepsinde veriliyor.

## Admin Ekranı Görselleri
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/12.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/13.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/14.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/15.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/16.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/17.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/18.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/19.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/20.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/21.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/22.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/23.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/24.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/25.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/26.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/27.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/28.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/29.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/30.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/31.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/32.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/33.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/34.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/35.PNG" height= 600>
<img src="https://github.com/mustafayakin/Dominos-Food-Ordering-App/blob/main/dominosPhotos/36.PNG" height= 600>
