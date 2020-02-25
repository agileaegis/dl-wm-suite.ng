Ογκος(It) 1100
Βάρος (Kg) 51
Ωφέλιμο φορτίο (Kg) 520
A Συνολικό Υψος  (mm) 1300
B Βάθος (mm) 1030
C Υψος μέχρι την επάνω κόγχη (mm) 1200
D Συνολικό Φάρδος  (mm) 1430
E Φάρδος  (mm) 1200
F  Διάμετρος τροχού (mm) 200
H Φάρδος Βάσης Τροχού (mm) 800
Υλικό HDPE


Ογκος(It) 360
Βάρος (Kg) 22
Ωφέλιμο φορτίο (Kg) 144
A Υψος μέχρι την επάνω κόγχη (mm) 1040
B  Συνολικό ύψος (mm) 1125
C  Βάθος (mm) 853
D  Συνολικό βάθος (mm) 715
F  Διάμετρος τροχού (mm) 200
Υλικό HDPE


MS10110

ΚΑΔΟΣ ΑΠΟΡΡΙΜΑΤΩΝ ΣΙΔΕΡΕΝΙΟΣ ΜΕ ΠΛΑΣΤΙΚΟ ΚΑΠΑΚΙ ΚΑΙ ΠΟΔΟΜΟΧΛΟ 1100LT / MS10110
Kωδικός Προϊόντος:
10100
Νέος σχεδιασμός  με νέα μπάρα ανύψωσης του κάδου με διπλές νευρώσεις για μεγάλη αντοχή στις καταπονήσεις και βιδωτούς μεντεσεδες.

1. Είναι μεταλλικοί τροχήλατοι, γαλβανιζέ, μεγάλης χωρητικότητας και αντοχής κατασκευασμένοι σύμφωνα με το Ευρωπαϊκό πρότυπο
κατά EN 840- 2/5/6.
2. Eίναι Ελληνικής και πρόσφατης κατασκευής όχι πέραν του έτους.
4. Έχουν σχήμα κόλουρης πυραμίδας με διευρυνόμενες πλευρές προς τα επάνω και απόλυτα στρογγυλεμένες γωνίες.
5. Ανταποκρίνονται απόλυτα κατά την ανάρτηση και ανατροπή, κατά περίπτωση, με τους μηχανισμούς ανύψωσης κάδων μηχανικής αποκομιδής οπίσθιας, 
πλάγιας και εμπροσθοπλάγιας φόρτωσης, οπτικής και μη επαφής.
6. Δέχονται άνετα και εύκολα ογκώδη, αιχμηρά και σκληρόκοκκα αντικείμενα.
7. Είναι φυσιολογικά αβλαβείς, απόλυτα ανθεκτικοί στην σήψη, στην διάβρωση και απρόσβλητοι από οξέα και χημικές ουσίες.


http://martinwilley.com/net/code/nhibernate/query2.html

http://martinwilley.com/net/code/nhibernate/query.html

https://docs.jboss.org/hibernate/orm/3.5/reference/en/html/queryhql.html

https://stackoverflow.com/questions/9836471/why-is-addrange-faster-than-using-a-foreach-loop


Potentially, AddRange can check where the value passed to it implements IList or IList<T>. If it does, it can find out how many values are in the range, and thus how much space it needs to allocate... whereas the foreach loop may need to reallocate several times.

Additionally, even after allocation, List<T> can use IList<T>.CopyTo to perform a bulk copy into the underlying array (for ranges which implement IList<T>, of course.)

I suspect you'll find that if you try your test again but using Enumerable.Range(0, 100000) for fillData instead of a List<T>, the two will take about the same time.