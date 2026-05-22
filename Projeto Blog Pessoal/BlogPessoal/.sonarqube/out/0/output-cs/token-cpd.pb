ﬁ/
sC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\UsuarioService.cs
	namespace 	
BlogPessoal
 
. 
Services 
; 
public		 
class		 
UsuarioService		 
:		 
IUsuarioService		 -
{

 
private 
readonly 
IUsuarioRepository '
_usuarioRepository( :
;: ;
private 
readonly 

JwtService 
_jwtService  +
;+ ,
public 

UsuarioService 
( 
IUsuarioRepository ,
usuarioRepository- >
,> ?

JwtService@ J

jwtServiceK U
)U V
{ 
_usuarioRepository 
= 
usuarioRepository .
;. /
_jwtService 
= 

jwtService  
;  !
} 
public 

async 
Task 
< 
IEnumerable !
<! "
Usuario" )
>) *
>* +
GetAllAsync, 7
(7 8
)8 9
{ 
return 
await 
_usuarioRepository '
.' (
GetAllAsync( 3
(3 4
)4 5
;5 6
} 
public 

async 
Task 
< 
Usuario 
? 
> 
GetByIdAsync  ,
(, -
long- 1
id2 4
)4 5
{ 
return 
await 
_usuarioRepository '
.' (
GetByIdAsync( 4
(4 5
id5 7
)7 8
;8 9
} 
public$$ 

async$$ 
Task$$ 
<$$ 
Usuario$$ 
?$$ 
>$$ 
CreateAsync$$  +
($$+ ,
Usuario$$, 3
usuario$$4 ;
)$$; <
{%% 
var'' 

emailEmUso'' 
='' 
await'' 
_usuarioRepository'' 1
.''1 2
EmailExistsAsync''2 B
(''B C
usuario''C J
.''J K
Email''K P
!''P Q
)''Q R
;''R S
if(( 

((( 

emailEmUso(( 
)(( 
return(( 
null(( #
;((# $
usuario++ 
.++ 
Senha++ 
=++ 
BCrypt++ 
.++ 
Net++ "
.++" #
BCrypt++# )
.++) *
HashPassword++* 6
(++6 7
usuario++7 >
.++> ?
Senha++? D
)++D E
;++E F
return-- 
await-- 
_usuarioRepository-- '
.--' (
CreateAsync--( 3
(--3 4
usuario--4 ;
)--; <
;--< =
}.. 
public33 

async33 
Task33 
<33 
Usuario33 
?33 
>33 
UpdateAsync33  +
(33+ ,
Usuario33, 3
usuario334 ;
)33; <
{44 
var55 
exists55 
=55 
await55 
_usuarioRepository55 -
.55- .
ExistsAsync55. 9
(559 :
usuario55: A
.55A B
Id55B D
)55D E
;55E F
if66 

(66 
!66 
exists66 
)66 
return66 
null66  
;66  !
usuario99 
.99 
Senha99 
=99 
BCrypt99 
.99 
Net99 "
.99" #
BCrypt99# )
.99) *
HashPassword99* 6
(996 7
usuario997 >
.99> ?
Senha99? D
)99D E
;99E F
return;; 
await;; 
_usuarioRepository;; '
.;;' (
UpdateAsync;;( 3
(;;3 4
usuario;;4 ;
);;; <
;;;< =
}<< 
public?? 

async?? 
Task?? 
<?? 
bool?? 
>?? 
DeleteAsync?? '
(??' (
long??( ,
id??- /
)??/ 0
{@@ 
varAA 
existsAA 
=AA 
awaitAA 
_usuarioRepositoryAA -
.AA- .
ExistsAsyncAA. 9
(AA9 :
idAA: <
)AA< =
;AA= >
ifBB 

(BB 
!BB 
existsBB 
)BB 
returnBB 
falseBB !
;BB! "
awaitDD 
_usuarioRepositoryDD  
.DD  !
DeleteAsyncDD! ,
(DD, -
idDD- /
)DD/ 0
;DD0 1
returnEE 
trueEE 
;EE 
}FF 
publicLL 

asyncLL 
TaskLL 
<LL 
stringLL 
?LL 
>LL 

LoginAsyncLL )
(LL) *
UsuarioLoginLL* 6
usuarioLoginLL7 C
)LLC D
{MM 
varOO 
usuarioOO 
=OO 
awaitOO 
_usuarioRepositoryOO .
.OO. /
GetByEmailAsyncOO/ >
(OO> ?
usuarioLoginOO? K
.OOK L
EmailOOL Q
!OOQ R
)OOR S
;OOS T
ifPP 

(PP 
usuarioPP 
isPP 
nullPP 
)PP 
returnPP #
nullPP$ (
;PP( )
varSS 
senhaValidaSS 
=SS 
BCryptSS  
.SS  !
NetSS! $
.SS$ %
BCryptSS% +
.SS+ ,
VerifySS, 2
(SS2 3
usuarioLoginSS3 ?
.SS? @
SenhaSS@ E
,SSE F
usuarioSSG N
.SSN O
SenhaSSO T
)SST U
;SSU V
ifTT 

(TT 
!TT 
senhaValidaTT 
)TT 
returnTT  
nullTT! %
;TT% &
returnWW 
_jwtServiceWW 
.WW 

GerarTokenWW %
(WW% &
usuarioWW& -
)WW- .
;WW. /
}XX 
}YY ˝
pC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\TemaService.cs
	namespace 	
BlogPessoal
 
. 
Services 
; 
public		 
class		 
TemaService		 
:		 
ITemaService		 '
{

 
private 
readonly 
ITemaRepository $
_temaRepository% 4
;4 5
public 

TemaService 
( 
ITemaRepository &
temaRepository' 5
)5 6
{ 
_temaRepository 
= 
temaRepository (
;( )
} 
public 

async 
Task 
< 
IEnumerable !
<! "
Tema" &
>& '
>' (
GetAllAsync) 4
(4 5
)5 6
{ 
return 
await 
_temaRepository $
.$ %
GetAllAsync% 0
(0 1
)1 2
;2 3
} 
public 

async 
Task 
< 
Tema 
? 
> 
GetByIdAsync )
() *
long* .
id/ 1
)1 2
{ 
return 
await 
_temaRepository $
.$ %
GetByIdAsync% 1
(1 2
id2 4
)4 5
;5 6
} 
public   

async   
Task   
<   
Tema   
>   
CreateAsync   '
(  ' (
Tema  ( ,
tema  - 1
)  1 2
{!! 
return"" 
await"" 
_temaRepository"" $
.""$ %
CreateAsync""% 0
(""0 1
tema""1 5
)""5 6
;""6 7
}## 
public'' 

async'' 
Task'' 
<'' 
Tema'' 
?'' 
>'' 
UpdateAsync'' (
(''( )
Tema'') -
tema''. 2
)''2 3
{(( 
var)) 
exists)) 
=)) 
await)) 
_temaRepository)) *
.))* +
ExistsAsync))+ 6
())6 7
tema))7 ;
.)); <
Id))< >
)))> ?
;))? @
if** 

(** 
!** 
exists** 
)** 
return** 
null**  
;**  !
return,, 
await,, 
_temaRepository,, $
.,,$ %
UpdateAsync,,% 0
(,,0 1
tema,,1 5
),,5 6
;,,6 7
}-- 
public11 

async11 
Task11 
<11 
bool11 
>11 
DeleteAsync11 '
(11' (
long11( ,
id11- /
)11/ 0
{22 
var33 
exists33 
=33 
await33 
_temaRepository33 *
.33* +
ExistsAsync33+ 6
(336 7
id337 9
)339 :
;33: ;
if44 

(44 
!44 
exists44 
)44 
return44 
false44 !
;44! "
await66 
_temaRepository66 
.66 
DeleteAsync66 )
(66) *
id66* ,
)66, -
;66- .
return77 
true77 
;77 
}88 
}99 ©D
tC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\PostagemService.cs
	namespace 	
BlogPessoal
 
. 
Services 
; 
public

 
class

 
PostagemService

 
:

 
IPostagemService

 /
{ 
private 
readonly 
IPostagemRepository (
_postagemRepository) <
;< =
private 
readonly 
ITemaRepository $
_temaRepository% 4
;4 5
private 
readonly 
IUsuarioRepository '
_usuarioRepository( :
;: ;
private 
readonly 

IIAService 

_iaService  *
;* +
public 

PostagemService 
( 
IPostagemRepository 
postagemRepository .
,. /
ITemaRepository 
temaRepository &
,& '
IUsuarioRepository 
usuarioRepository ,
,, -

IIAService 
	iaService 
) 
{ 
_postagemRepository 
= 
postagemRepository 0
;0 1
_temaRepository 
= 
temaRepository (
;( )
_usuarioRepository 
= 
usuarioRepository .
;. /

_iaService 
= 
	iaService 
; 
} 
public 

async 
Task 
< 
IEnumerable !
<! "
Postagem" *
>* +
>+ ,
GetAllAsync- 8
(8 9
)9 :
{   
return!! 
await!! 
_postagemRepository!! (
.!!( )
GetAllAsync!!) 4
(!!4 5
)!!5 6
;!!6 7
}"" 
public%% 

async%% 
Task%% 
<%% 
Postagem%% 
?%% 
>%%  
GetByIdAsync%%! -
(%%- .
long%%. 2
id%%3 5
)%%5 6
{&& 
return'' 
await'' 
_postagemRepository'' (
.''( )
GetByIdAsync'') 5
(''5 6
id''6 8
)''8 9
;''9 :
}(( 
public++ 

async++ 
Task++ 
<++ 
IEnumerable++ !
<++! "
Postagem++" *
>++* +
>+++ ,
GetByAutorAsync++- <
(++< =
long++= A
	usuarioId++B K
)++K L
{,, 
return-- 
await-- 
_postagemRepository-- (
.--( )
GetByAutorAsync--) 8
(--8 9
	usuarioId--9 B
)--B C
;--C D
}.. 
public11 

async11 
Task11 
<11 
IEnumerable11 !
<11! "
Postagem11" *
>11* +
>11+ ,
GetByTemaAsync11- ;
(11; <
long11< @
temaId11A G
)11G H
{22 
return33 
await33 
_postagemRepository33 (
.33( )
GetByTemaAsync33) 7
(337 8
temaId338 >
)33> ?
;33? @
}44 
public:: 

async:: 
Task:: 
<:: 
Postagem:: 
?:: 
>::  
CreateAsync::! ,
(::, -
Postagem::- 5
postagem::6 >
)::> ?
{;; 
if== 

(== 
postagem== 
.== 
Tema== 
is== 
not==  
null==! %
)==% &
{>> 	
var?? 

temaExists?? 
=?? 
await?? "
_temaRepository??# 2
.??2 3
ExistsAsync??3 >
(??> ?
postagem??? G
.??G H
Tema??H L
.??L M
Id??M O
)??O P
;??P Q
if@@ 
(@@ 
!@@ 

temaExists@@ 
)@@ 
return@@ #
null@@$ (
;@@( )
}AA 	
ifDD 

(DD 
postagemDD 
.DD 
UsuarioDD 
isDD 
notDD  #
nullDD$ (
)DD( )
{EE 	
varFF 
usuarioExistsFF 
=FF 
awaitFF  %
_usuarioRepositoryFF& 8
.FF8 9
ExistsAsyncFF9 D
(FFD E
postagemFFE M
.FFM N
UsuarioFFN U
.FFU V
IdFFV X
)FFX Y
;FFY Z
ifGG 
(GG 
!GG 
usuarioExistsGG 
)GG 
returnGG  &
nullGG' +
;GG+ ,
}HH 	
postagemKK 
.KK 
DataKK 
=KK 
DateTimeKK  
.KK  !
UtcNowKK! '
;KK' (
varNN 
resultadoIANN 
=NN 
awaitNN 

_iaServiceNN  *
.NN* +
GerarResumoAsyncNN+ ;
(NN; <
postagemNN< D
.NND E
TextoNNE J
!NNJ K
)NNK L
;NNL M
postagemOO 
.OO 
ResumoIAOO 
=OO 
resultadoIAOO )
.OO) *
ResumoOO* 0
;OO0 1
postagemPP 
.PP 
TagsIAPP 
=PP 
resultadoIAPP )
.PP) *
TagsPP* .
;PP. /
postagemQQ 
.QQ 
CategoriaIAQQ 
=QQ 
resultadoIAQQ *
.QQ* +
	CategoriaQQ+ 4
;QQ4 5
returnSS 
awaitSS 
_postagemRepositorySS (
.SS( )
CreateAsyncSS) 4
(SS4 5
postagemSS5 =
)SS= >
;SS> ?
}TT 
publicYY 

asyncYY 
TaskYY 
<YY 
PostagemYY 
?YY 
>YY  
UpdateAsyncYY! ,
(YY, -
PostagemYY- 5
postagemYY6 >
)YY> ?
{ZZ 
var\\ 
exists\\ 
=\\ 
await\\ 
_postagemRepository\\ .
.\\. /
ExistsAsync\\/ :
(\\: ;
postagem\\; C
.\\C D
Id\\D F
)\\F G
;\\G H
if]] 

(]] 
!]] 
exists]] 
)]] 
return]] 
null]]  
;]]  !
if`` 

(`` 
postagem`` 
.`` 
Tema`` 
is`` 
not``  
null``! %
)``% &
{aa 	
varbb 

temaExistsbb 
=bb 
awaitbb "
_temaRepositorybb# 2
.bb2 3
ExistsAsyncbb3 >
(bb> ?
postagembb? G
.bbG H
TemabbH L
.bbL M
IdbbM O
)bbO P
;bbP Q
ifcc 
(cc 
!cc 

temaExistscc 
)cc 
returncc #
nullcc$ (
;cc( )
}dd 	
ifgg 

(gg 
postagemgg 
.gg 
Usuariogg 
isgg 
notgg  #
nullgg$ (
)gg( )
{hh 	
varii 
usuarioExistsii 
=ii 
awaitii  %
_usuarioRepositoryii& 8
.ii8 9
ExistsAsyncii9 D
(iiD E
postagemiiE M
.iiM N
UsuarioiiN U
.iiU V
IdiiV X
)iiX Y
;iiY Z
ifjj 
(jj 
!jj 
usuarioExistsjj 
)jj 
returnjj  &
nulljj' +
;jj+ ,
}kk 	
postagemnn 
.nn 
Datann 
=nn 
DateTimenn  
.nn  !
UtcNownn! '
;nn' (
returnpp 
awaitpp 
_postagemRepositorypp (
.pp( )
UpdateAsyncpp) 4
(pp4 5
postagempp5 =
)pp= >
;pp> ?
}qq 
publictt 

asynctt 
Tasktt 
<tt 
booltt 
>tt 
DeleteAsynctt '
(tt' (
longtt( ,
idtt- /
)tt/ 0
{uu 
varvv 
existsvv 
=vv 
awaitvv 
_postagemRepositoryvv .
.vv. /
ExistsAsyncvv/ :
(vv: ;
idvv; =
)vv= >
;vv> ?
ifww 

(ww 
!ww 
existsww 
)ww 
returnww 
falseww !
;ww! "
awaityy 
_postagemRepositoryyy !
.yy! "
DeleteAsyncyy" -
(yy- .
idyy. 0
)yy0 1
;yy1 2
returnzz 
truezz 
;zz 
}{{ 
}|| ≠
oC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\JwtService.cs
	namespace 	
BlogPessoal
 
. 
Services 
; 
public 
class 

JwtService 
{ 
private 
readonly 
IConfiguration #
_configuration$ 2
;2 3
public 


JwtService 
( 
IConfiguration $
configuration% 2
)2 3
{ 
_configuration 
= 
configuration &
;& '
} 
public 

string 

GerarToken 
( 
Usuario $
usuario% ,
), -
{ 
var 
key 
= 
new  
SymmetricSecurityKey *
(* +
Encoding 
. 
UTF8 
. 
GetBytes "
(" #
_configuration# 1
[1 2
$str2 ;
]; <
!< =
)= >
)> ?
;? @
var 
credentials 
= 
new 
SigningCredentials 0
(0 1
key 
, 
SecurityAlgorithms #
.# $

HmacSha256$ .
). /
;/ 0
var 
claims 
= 
new 
[ 
] 
{ 	
new   
Claim   
(   

ClaimTypes    
.    !
NameIdentifier  ! /
,  / 0
usuario  1 8
.  8 9
Id  9 ;
.  ; <
ToString  < D
(  D E
)  E F
)  F G
,  G H
new!! 
Claim!! 
(!! 

ClaimTypes!!  
.!!  !
Email!!! &
,!!& '
usuario!!( /
.!!/ 0
Email!!0 5
!!!5 6
)!!6 7
,!!7 8
new"" 
Claim"" 
("" 

ClaimTypes""  
.""  !
Name""! %
,""% &
usuario""' .
."". /
Nome""/ 3
!""3 4
)""4 5
,""5 6
}## 	
;##	 

var%% 
token%% 
=%% 
new%% 
JwtSecurityToken%% (
(%%( )
issuer&& 
:&& 
_configuration&& "
[&&" #
$str&&# /
]&&/ 0
,&&0 1
audience'' 
:'' 
_configuration'' $
[''$ %
$str''% 3
]''3 4
,''4 5
claims(( 
:(( 
claims(( 
,(( 
expires)) 
:)) 
DateTime)) 
.)) 
UtcNow)) $
.))$ %
AddHours))% -
())- .
$num)). /
)))/ 0
,))0 1
signingCredentials** 
:** 
credentials**  +
)++ 	
;++	 

return-- 
new-- #
JwtSecurityTokenHandler-- *
(--* +
)--+ ,
.--, -

WriteToken--- 7
(--7 8
token--8 =
)--= >
;--> ?
}.. 
}// ∂
uC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\IUsuarioServices.cs
	namespace 	
BlogPessoal
 
. 
Services 
; 
public 
	interface 
IUsuarioService  
{		 
Task 
< 	
IEnumerable	 
< 
Usuario 
> 
> 
GetAllAsync *
(* +
)+ ,
;, -
Task 
< 	
Usuario	 
? 
> 
GetByIdAsync 
(  
long  $
id% '
)' (
;( )
Task 
< 	
Usuario	 
? 
> 
CreateAsync 
( 
Usuario &
usuario' .
). /
;/ 0
Task 
< 	
Usuario	 
? 
> 
UpdateAsync 
( 
Usuario &
usuario' .
). /
;/ 0
Task 
< 	
bool	 
> 
DeleteAsync 
( 
long 
id  "
)" #
;# $
Task 
< 	
string	 
? 
> 

LoginAsync 
( 
UsuarioLogin )
usuarioLogin* 6
)6 7
;7 8
} Ω	
rC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\ITemaServices.cs
	namespace 	
BlogPessoal
 
. 
Services 
; 
public 
	interface 
ITemaService 
{		 
Task 
< 	
IEnumerable	 
< 
Tema 
> 
> 
GetAllAsync '
(' (
)( )
;) *
Task 
< 	
Tema	 
? 
> 
GetByIdAsync 
( 
long !
id" $
)$ %
;% &
Task 
< 	
Tema	 
> 
CreateAsync 
( 
Tema 
tema  $
)$ %
;% &
Task 
< 	
Tema	 
? 
> 
UpdateAsync 
( 
Tema  
tema! %
)% &
;& '
Task 
< 	
bool	 
> 
DeleteAsync 
( 
long 
id  "
)" #
;# $
} œ
uC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\IPostagemService.cs
	namespace 	
BlogPessoal
 
. 
Services 
; 
public 
	interface 
IPostagemService !
{		 
Task 
< 	
IEnumerable	 
< 
Postagem 
> 
> 
GetAllAsync  +
(+ ,
), -
;- .
Task 
< 	
Postagem	 
? 
> 
GetByIdAsync  
(  !
long! %
id& (
)( )
;) *
Task 
< 	
IEnumerable	 
< 
Postagem 
> 
> 
GetByAutorAsync  /
(/ 0
long0 4
	usuarioId5 >
)> ?
;? @
Task 
< 	
IEnumerable	 
< 
Postagem 
> 
> 
GetByTemaAsync  .
(. /
long/ 3
temaId4 :
): ;
;; <
Task 
< 	
Postagem	 
? 
> 
CreateAsync 
(  
Postagem  (
postagem) 1
)1 2
;2 3
Task 
< 	
Postagem	 
? 
> 
UpdateAsync 
(  
Postagem  (
postagem) 1
)1 2
;2 3
Task   
<   	
bool  	 
>   
DeleteAsync   
(   
long   
id    "
)  " #
;  # $
}!! ¡
sC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\IA\PromptBuild.cs
	namespace 	
BlogPessoal
 
. 
Services 
. 
IA !
;! "
public 
static 
class 
PromptBuilder !
{ 
public

 

static

 
string

 
BuildResumoPrompt

 *
(

* +
string

+ 1
conteudo

2 :
)

: ;
{ 
return 

$str _
+` a
$str B
+C D
$str 6
+7 8
$str <
+= >
$str -
+. /
$str 9
+: ;
$str  
+! "
conteudo# +
;+ ,
} 
} è
sC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\IA\IIAServices.cs
	namespace 	
BlogPessoal
 
. 
Services 
. 
IA !
;! "
public 
	interface 

IIAService 
{		 
Task 
< 	
ResultadoIA	 
> 
GerarResumoAsync &
(& '
string' -
conteudo. 6
)6 7
;7 8
} Ã
{C:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Repositories\IUsuarioRepository.cs
	namespace 	
BlogPessoal
 
. 
Repositories "
;" #
public 
	interface 
IUsuarioRepository #
{		 
Task 
< 	
IEnumerable	 
< 
Usuario 
> 
> 
GetAllAsync *
(* +
)+ ,
;, -
Task 
< 	
Usuario	 
? 
> 
GetByIdAsync 
(  
long  $
id% '
)' (
;( )
Task 
< 	
Usuario	 
? 
> 
GetByEmailAsync "
(" #
string# )
email* /
)/ 0
;0 1
Task 
< 	
Usuario	 
> 
CreateAsync 
( 
Usuario %
usuario& -
)- .
;. /
Task 
< 	
Usuario	 
> 
UpdateAsync 
( 
Usuario %
usuario& -
)- .
;. /
Task 
DeleteAsync	 
( 
long 
id 
) 
; 
Task 
< 	
bool	 
> 
ExistsAsync 
( 
long 
id  "
)" #
;# $
Task   
<   	
bool  	 
>   
EmailExistsAsync   
(    
string    &
email  ' ,
)  , -
;  - .
}!! ‹(
zC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Repositories\UsuarioRepository.cs
	namespace 	
BlogPessoal
 
. 
Repositories "
;" #
public

 
class

 
UsuarioRepository

 
:

  
IUsuarioRepository

! 3
{ 
private 
readonly 
AppDbContext !
_context" *
;* +
public 

UsuarioRepository 
( 
AppDbContext )
context* 1
)1 2
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
IEnumerable !
<! "
Usuario" )
>) *
>* +
GetAllAsync, 7
(7 8
)8 9
{ 
return 
await 
_context 
. 
Usuarios &
.& '
ToListAsync' 2
(2 3
)3 4
;4 5
} 
public 

async 
Task 
< 
Usuario 
? 
> 
GetByIdAsync  ,
(, -
long- 1
id2 4
)4 5
{ 
return 
await 
_context 
. 
Usuarios &
. 
FirstOrDefaultAsync  
(  !
u! "
=># %
u& '
.' (
Id( *
==+ -
id. 0
)0 1
;1 2
} 
public"" 

async"" 
Task"" 
<"" 
Usuario"" 
?"" 
>"" 
GetByEmailAsync""  /
(""/ 0
string""0 6
email""7 <
)""< =
{## 
return$$ 
await$$ 
_context$$ 
.$$ 
Usuarios$$ &
.%% 
FirstOrDefaultAsync%%  
(%%  !
u%%! "
=>%%# %
u%%& '
.%%' (
Email%%( -
==%%. 0
email%%1 6
)%%6 7
;%%7 8
}&& 
public** 

async** 
Task** 
<** 
Usuario** 
>** 
CreateAsync** *
(*** +
Usuario**+ 2
usuario**3 :
)**: ;
{++ 
_context,, 
.,, 
Usuarios,, 
.,, 
Add,, 
(,, 
usuario,, %
),,% &
;,,& '
await-- 
_context-- 
.-- 
SaveChangesAsync-- '
(--' (
)--( )
;--) *
return.. 
usuario.. 
;.. 
}// 
public22 

async22 
Task22 
<22 
Usuario22 
>22 
UpdateAsync22 *
(22* +
Usuario22+ 2
usuario223 :
)22: ;
{33 
_context44 
.44 
Usuarios44 
.44 
Update44  
(44  !
usuario44! (
)44( )
;44) *
await55 
_context55 
.55 
SaveChangesAsync55 '
(55' (
)55( )
;55) *
return66 
usuario66 
;66 
}77 
public:: 

async:: 
Task:: 
DeleteAsync:: !
(::! "
long::" &
id::' )
)::) *
{;; 
var<< 
usuario<< 
=<< 
await<< 
GetByIdAsync<< (
(<<( )
id<<) +
)<<+ ,
;<<, -
if== 

(== 
usuario== 
is== 
not== 
null== 
)==  
{>> 	
_context?? 
.?? 
Usuarios?? 
.?? 
Remove?? $
(??$ %
usuario??% ,
)??, -
;??- .
await@@ 
_context@@ 
.@@ 
SaveChangesAsync@@ +
(@@+ ,
)@@, -
;@@- .
}AA 	
}BB 
publicEE 

asyncEE 
TaskEE 
<EE 
boolEE 
>EE 
ExistsAsyncEE '
(EE' (
longEE( ,
idEE- /
)EE/ 0
{FF 
returnGG 
awaitGG 
_contextGG 
.GG 
UsuariosGG &
.GG& '
AnyAsyncGG' /
(GG/ 0
uGG0 1
=>GG2 4
uGG5 6
.GG6 7
IdGG7 9
==GG: <
idGG= ?
)GG? @
;GG@ A
}HH 
publicLL 

asyncLL 
TaskLL 
<LL 
boolLL 
>LL 
EmailExistsAsyncLL ,
(LL, -
stringLL- 3
emailLL4 9
)LL9 :
{MM 
returnNN 
awaitNN 
_contextNN 
.NN 
UsuariosNN &
.NN& '
AnyAsyncNN' /
(NN/ 0
uNN0 1
=>NN2 4
uNN5 6
.NN6 7
EmailNN7 <
==NN= ?
emailNN@ E
)NNE F
;NNF G
}OO 
}PP ”
wC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Repositories\TemaRepository.cs
	namespace 	
BlogPessoal
 
. 
Repositories "
;" #
public

 
class

 
TemaRepository

 
:

 
ITemaRepository

 -
{ 
private 
readonly 
AppDbContext !
_context" *
;* +
public 

TemaRepository 
( 
AppDbContext &
context' .
). /
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
IEnumerable !
<! "
Tema" &
>& '
>' (
GetAllAsync) 4
(4 5
)5 6
{ 
return 
await 
_context 
. 
Temas #
.# $
ToListAsync$ /
(/ 0
)0 1
;1 2
} 
public 

async 
Task 
< 
Tema 
? 
> 
GetByIdAsync )
() *
long* .
id/ 1
)1 2
{ 
return 
await 
_context 
. 
Temas #
.# $
FirstOrDefaultAsync$ 7
(7 8
t8 9
=>: <
t= >
.> ?
Id? A
==B D
idE G
)G H
;H I
} 
public"" 

async"" 
Task"" 
<"" 
Tema"" 
>"" 
CreateAsync"" '
(""' (
Tema""( ,
tema""- 1
)""1 2
{## 
_context$$ 
.$$ 
Temas$$ 
.$$ 
Add$$ 
($$ 
tema$$ 
)$$  
;$$  !
await%% 
_context%% 
.%% 
SaveChangesAsync%% '
(%%' (
)%%( )
;%%) *
return&& 
tema&& 
;&& 
}'' 
public** 

async** 
Task** 
<** 
Tema** 
>** 
UpdateAsync** '
(**' (
Tema**( ,
tema**- 1
)**1 2
{++ 
_context,, 
.,, 
Temas,, 
.,, 
Update,, 
(,, 
tema,, "
),," #
;,,# $
await-- 
_context-- 
.-- 
SaveChangesAsync-- '
(--' (
)--( )
;--) *
return.. 
tema.. 
;.. 
}// 
public22 

async22 
Task22 
DeleteAsync22 !
(22! "
long22" &
id22' )
)22) *
{33 
var44 
tema44 
=44 
await44 
GetByIdAsync44 %
(44% &
id44& (
)44( )
;44) *
if55 

(55 
tema55 
is55 
not55 
null55 
)55 
{66 	
_context77 
.77 
Temas77 
.77 
Remove77 !
(77! "
tema77" &
)77& '
;77' (
await88 
_context88 
.88 
SaveChangesAsync88 +
(88+ ,
)88, -
;88- .
}99 	
}:: 
public== 

async== 
Task== 
<== 
bool== 
>== 
ExistsAsync== '
(==' (
long==( ,
id==- /
)==/ 0
{>> 
return?? 
await?? 
_context?? 
.?? 
Temas?? #
.??# $
AnyAsync??$ ,
(??, -
t??- .
=>??/ 1
t??2 3
.??3 4
Id??4 6
==??7 9
id??: <
)??< =
;??= >
}@@ 
}AA Ë7
uC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Services\IA\GeminiService.cs
	namespace 	
BlogPessoal
 
. 
Services 
. 
IA !
;! "
public

 
class

 
GeminiService

 
:

 

IIAService

 '
{ 
private 
const 
string 
CategoriaDefault )
=* +
$str, 3
;3 4
private 
static 
readonly !
JsonSerializerOptions 1
_jsonOptions2 >
=? @
new 
( 
) 
{ '
PropertyNameCaseInsensitive +
=, -
true. 2
}3 4
;4 5
private 
readonly 

HttpClient 
_httpClient  +
;+ ,
private 
readonly 
string 
_apiKey #
;# $
public 

GeminiService 
( 

HttpClient #

httpClient$ .
,. /
IConfiguration0 >
configuration? L
)L M
{ 
_httpClient 
= 

httpClient  
;  !
_apiKey 
= 
configuration 
[  
$str  /
]/ 0
!0 1
;1 2
} 
public 

async 
Task 
< 
ResultadoIA !
>! "
GerarResumoAsync# 3
(3 4
string4 :
conteudo; C
)C D
{ 
var 
prompt 
= 
PromptBuilder "
." #
BuildResumoPrompt# 4
(4 5
conteudo5 =
)= >
;> ?
var 
requestBody 
= 
new 
{ 	
contents   
=   
new   
[   
]   
{!! 
new"" 
{"" 
parts"" 
="" 
new"" !
[""! "
]""" #
{""$ %
new""& )
{""* +
text"", 0
=""1 2
prompt""3 9
}"": ;
}""< =
}""> ?
}## 
}$$ 	
;$$	 

var&& 
json&& 
=&& 
JsonSerializer&& !
.&&! "
	Serialize&&" +
(&&+ ,
requestBody&&, 7
)&&7 8
;&&8 9
var'' 
content'' 
='' 
new'' 
StringContent'' '
(''' (
json''( ,
,'', -
Encoding''. 6
.''6 7
UTF8''7 ;
,''; <
$str''= O
)''O P
;''P Q
var)) 
url)) 
=)) 
$")) 
$str)) q
{))q r
_apiKey))r y
}))y z
"))z {
;)){ |
var** 
response** 
=** 
await** 
_httpClient** (
.**( )
	PostAsync**) 2
(**2 3
url**3 6
,**6 7
content**8 ?
)**? @
;**@ A
if-- 

(-- 
!-- 
response-- 
.-- 
IsSuccessStatusCode-- )
)--) *
{.. 	
return// 
new// 
ResultadoIA// "
{00 
Resumo11 
=11 
$"11 
$str11 7
{117 8
(118 9
int119 <
)11< =
response11= E
.11E F

StatusCode11F P
}11P Q
$str11Q S
"11S T
,11T U
Tags22 
=22 
$str22 
,22 
	Categoria33 
=33 
CategoriaDefault33 ,
}44 
;44 
}55 	
var77 
responseBody77 
=77 
await77  
response77! )
.77) *
Content77* 1
.771 2
ReadAsStringAsync772 C
(77C D
)77D E
;77E F
try99 
{:: 	
using;; 
var;; 
doc;; 
=;; 
JsonDocument;; (
.;;( )
Parse;;) .
(;;. /
responseBody;;/ ;
);;; <
;;;< =
var== 
text== 
=== 
doc== 
.== 
RootElement== &
.>> 
GetProperty>> 
(>> 
$str>> )
)>>) *
[>>* +
$num>>+ ,
]>>, -
.?? 
GetProperty?? 
(?? 
$str?? &
)??& '
.@@ 
GetProperty@@ 
(@@ 
$str@@ $
)@@$ %
[@@% &
$num@@& '
]@@' (
.AA 
GetPropertyAA 
(AA 
$strAA #
)AA# $
.BB 
	GetStringBB 
(BB 
)BB 
!BB 
;BB 
textDD 
=DD 
textDD 
.DD 
TrimDD 
(DD 
)DD 
.DD 
	TrimStartDD (
(DD( )
$charDD) ,
)DD, -
.DD- .
TrimEndDD. 5
(DD5 6
$charDD6 9
)DD9 :
;DD: ;
ifEE 
(EE 
textEE 
.EE 

StartsWithEE 
(EE  
$strEE  &
)EE& '
)EE' (
textEE) -
=EE. /
textEE0 4
[EE4 5
$numEE5 6
..EE6 8
]EE8 9
.EE9 :
TrimEE: >
(EE> ?
)EE? @
;EE@ A
varGG 
	resultadoGG 
=GG 
JsonSerializerGG *
.GG* +
DeserializeGG+ 6
<GG6 7
ResultadoIAGG7 B
>GGB C
(GGC D
textGGD H
,GGH I
_jsonOptionsGGJ V
)GGV W
;GGW X
returnII 
	resultadoII 
??II 
newII  #
ResultadoIAII$ /
{JJ 
ResumoKK 
=KK 
$strKK ;
,KK; <
TagsLL 
=LL 
$strLL 
,LL 
	CategoriaMM 
=MM 
CategoriaDefaultMM ,
}NN 
;NN 
}OO 	
catchPP 
(PP 
JsonExceptionPP 
)PP 
{QQ 	
returnRR 
newRR 
ResultadoIARR "
{SS 
ResumoTT 
=TT 
$strTT 3
,TT3 4
TagsUU 
=UU 
$strUU 
,UU 
	CategoriaVV 
=VV 
CategoriaDefaultVV ,
}WW 
;WW 
}XX 	
catchYY 
(YY %
InvalidOperationExceptionYY (
)YY( )
{ZZ 	
return[[ 
new[[ 
ResultadoIA[[ "
{\\ 
Resumo]] 
=]] 
$str]] B
,]]B C
Tags^^ 
=^^ 
$str^^ 
,^^ 
	Categoria__ 
=__ 
CategoriaDefault__ ,
}`` 
;`` 
}aa 	
}bb 
}cc ´C
{C:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Repositories\PostagemRepository.cs
	namespace 	
BlogPessoal
 
. 
Repositories "
;" #
public

 
class

 
PostagemRepository

 
:

  !
IPostagemRepository

" 5
{ 
private 
readonly 
AppDbContext !
_context" *
;* +
public 

PostagemRepository 
( 
AppDbContext *
context+ 2
)2 3
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
IEnumerable !
<! "
Postagem" *
>* +
>+ ,
GetAllAsync- 8
(8 9
)9 :
{ 
return 
await 
_context 
. 
	Postagens '
. 
Include 
( 
p 
=> 
p 
. 
Tema  
)  !
. 
Include 
( 
p 
=> 
p 
. 
Usuario #
)# $
. 
ToListAsync 
( 
) 
; 
} 
public 

async 
Task 
< 
Postagem 
? 
>  
GetByIdAsync! -
(- .
long. 2
id3 5
)5 6
{   
return!! 
await!! 
_context!! 
.!! 
	Postagens!! '
."" 
Include"" 
("" 
p"" 
=>"" 
p"" 
."" 
Tema""  
)""  !
.## 
Include## 
(## 
p## 
=>## 
p## 
.## 
Usuario## #
)### $
.$$ 
FirstOrDefaultAsync$$  
($$  !
p$$! "
=>$$# %
p$$& '
.$$' (
Id$$( *
==$$+ -
id$$. 0
)$$0 1
;$$1 2
}%% 
public)) 

async)) 
Task)) 
<)) 
IEnumerable)) !
<))! "
Postagem))" *
>))* +
>))+ ,
GetByAutorAsync))- <
())< =
long))= A
	usuarioId))B K
)))K L
{** 
return++ 
await++ 
_context++ 
.++ 
	Postagens++ '
.,, 
Include,, 
(,, 
p,, 
=>,, 
p,, 
.,, 
Tema,,  
),,  !
.-- 
Include-- 
(-- 
p-- 
=>-- 
p-- 
.-- 
Usuario-- #
)--# $
... 
Where.. 
(.. 
p.. 
=>.. 
p.. 
... 
Usuario.. !
!..! "
..." #
Id..# %
==..& (
	usuarioId..) 2
)..2 3
.// 
ToListAsync// 
(// 
)// 
;// 
}00 
public44 

async44 
Task44 
<44 
IEnumerable44 !
<44! "
Postagem44" *
>44* +
>44+ ,
GetByTemaAsync44- ;
(44; <
long44< @
temaId44A G
)44G H
{55 
return66 
await66 
_context66 
.66 
	Postagens66 '
.77 
Include77 
(77 
p77 
=>77 
p77 
.77 
Tema77  
)77  !
.88 
Include88 
(88 
p88 
=>88 
p88 
.88 
Usuario88 #
)88# $
.99 
Where99 
(99 
p99 
=>99 
p99 
.99 
Tema99 
!99 
.99  
Id99  "
==99# %
temaId99& ,
)99, -
.:: 
ToListAsync:: 
(:: 
):: 
;:: 
};; 
public?? 

async?? 
Task?? 
<?? 
Postagem?? 
>?? 
CreateAsync??  +
(??+ ,
Postagem??, 4
postagem??5 =
)??= >
{@@ 
ifCC 

(CC 
postagemCC 
.CC 
TemaCC 
isCC 
notCC  
nullCC! %
)CC% &
postagemDD 
.DD 
TemaDD 
=DD 
awaitDD !
_contextDD" *
.DD* +
TemasDD+ 0
.EE 
	FindAsyncEE 
(EE 
postagemEE #
.EE# $
TemaEE$ (
.EE( )
IdEE) +
)EE+ ,
;EE, -
ifGG 

(GG 
postagemGG 
.GG 
UsuarioGG 
isGG 
notGG  #
nullGG$ (
)GG( )
postagemHH 
.HH 
UsuarioHH 
=HH 
awaitHH $
_contextHH% -
.HH- .
UsuariosHH. 6
.II 
	FindAsyncII 
(II 
postagemII #
.II# $
UsuarioII$ +
.II+ ,
IdII, .
)II. /
;II/ 0
_contextKK 
.KK 
	PostagensKK 
.KK 
AddKK 
(KK 
postagemKK '
)KK' (
;KK( )
awaitLL 
_contextLL 
.LL 
SaveChangesAsyncLL '
(LL' (
)LL( )
;LL) *
returnMM 
postagemMM 
;MM 
}NN 
publicPP 

asyncPP 
TaskPP 
<PP 
PostagemPP 
>PP 
UpdateAsyncPP  +
(PP+ ,
PostagemPP, 4
postagemPP5 =
)PP= >
{QQ 
ifTT 

(TT 
postagemTT 
.TT 
TemaTT 
isTT 
notTT  
nullTT! %
)TT% &
postagemUU 
.UU 
TemaUU 
=UU 
awaitUU !
_contextUU" *
.UU* +
TemasUU+ 0
.VV 
	FindAsyncVV 
(VV 
postagemVV #
.VV# $
TemaVV$ (
.VV( )
IdVV) +
)VV+ ,
;VV, -
ifXX 

(XX 
postagemXX 
.XX 
UsuarioXX 
isXX 
notXX  #
nullXX$ (
)XX( )
postagemYY 
.YY 
UsuarioYY 
=YY 
awaitYY $
_contextYY% -
.YY- .
UsuariosYY. 6
.ZZ 
	FindAsyncZZ 
(ZZ 
postagemZZ #
.ZZ# $
UsuarioZZ$ +
.ZZ+ ,
IdZZ, .
)ZZ. /
;ZZ/ 0
_context\\ 
.\\ 
	Postagens\\ 
.\\ 
Update\\ !
(\\! "
postagem\\" *
)\\* +
;\\+ ,
await]] 
_context]] 
.]] 
SaveChangesAsync]] '
(]]' (
)]]( )
;]]) *
return^^ 
postagem^^ 
;^^ 
}__ 
publicbb 

asyncbb 
Taskbb 
DeleteAsyncbb !
(bb! "
longbb" &
idbb' )
)bb) *
{cc 
vardd 
postagemdd 
=dd 
awaitdd 
GetByIdAsyncdd )
(dd) *
iddd* ,
)dd, -
;dd- .
ifee 

(ee 
postagemee 
isee 
notee 
nullee  
)ee  !
{ff 	
_contextgg 
.gg 
	Postagensgg 
.gg 
Removegg %
(gg% &
postagemgg& .
)gg. /
;gg/ 0
awaithh 
_contexthh 
.hh 
SaveChangesAsynchh +
(hh+ ,
)hh, -
;hh- .
}ii 	
}jj 
publicnn 

asyncnn 
Tasknn 
<nn 
boolnn 
>nn 
ExistsAsyncnn '
(nn' (
longnn( ,
idnn- /
)nn/ 0
{oo 
returnpp 
awaitpp 
_contextpp 
.pp 
	Postagenspp '
.pp' (
AnyAsyncpp( 0
(pp0 1
ppp1 2
=>pp3 5
ppp6 7
.pp7 8
Idpp8 :
==pp; =
idpp> @
)pp@ A
;ppA B
}qq 
}rr µ

xC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Repositories\ITemaRepository.cs
	namespace 	
BlogPessoal
 
. 
Repositories "
;" #
public 
	interface 
ITemaRepository  
{		 
Task 
< 	
IEnumerable	 
< 
Tema 
> 
> 
GetAllAsync '
(' (
)( )
;) *
Task 
< 	
Tema	 
? 
> 
GetByIdAsync 
( 
long !
id" $
)$ %
;% &
Task 
< 	
Tema	 
> 
CreateAsync 
( 
Tema 
tema  $
)$ %
;% &
Task 
< 	
Tema	 
> 
UpdateAsync 
( 
Tema 
tema  $
)$ %
;% &
Task 
DeleteAsync	 
( 
long 
id 
) 
; 
Task 
< 	
bool	 
> 
ExistsAsync 
( 
long 
id  "
)" #
;# $
} π
|C:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Repositories\IPostagemRepository.cs
	namespace 	
BlogPessoal
 
. 
Repositories "
;" #
public 
	interface 
IPostagemRepository $
{		 
Task 
< 	
IEnumerable	 
< 
Postagem 
> 
> 
GetAllAsync  +
(+ ,
), -
;- .
Task 
< 	
Postagem	 
? 
> 
GetByIdAsync  
(  !
long! %
id& (
)( )
;) *
Task 
< 	
IEnumerable	 
< 
Postagem 
> 
> 
GetByAutorAsync  /
(/ 0
long0 4
	usuarioId5 >
)> ?
;? @
Task 
< 	
IEnumerable	 
< 
Postagem 
> 
> 
GetByTemaAsync  .
(. /
long/ 3
temaId4 :
): ;
;; <
Task 
< 	
Postagem	 
> 
CreateAsync 
( 
Postagem '
postagem( 0
)0 1
;1 2
Task 
< 	
Postagem	 
> 
UpdateAsync 
( 
Postagem '
postagem( 0
)0 1
;1 2
Task 
DeleteAsync	 
( 
long 
id 
) 
; 
Task   
<   	
bool  	 
>   
ExistsAsync   
(   
long   
id    "
)  " #
;  # $
}!! ◊@
cC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
var 
connectionString 
= 
builder 
. 
Configuration ,
. 
GetConnectionString 
( 
$str ,
), -
;- .
builder 
. 
Services 
. 
AddDbContext 
< 
AppDbContext *
>* +
(+ ,
options, 3
=>4 6
options 
. 
UseMySql 
( 
connectionString %
,% &
ServerVersion 
. 

AutoDetect  
(  !
connectionString! 1
)1 2
)2 3
)3 4
;4 5
var 
jwtKey 

= 
builder 
. 
Configuration "
[" #
$str# ,
], -
!- .
;. /
builder 
. 
Services 
. 
AddAuthentication "
(" #
options# *
=>+ -
{ 
options 
. %
DefaultAuthenticateScheme %
=& '
JwtBearerDefaults( 9
.9 : 
AuthenticationScheme: N
;N O
options 
. "
DefaultChallengeScheme "
=# $
JwtBearerDefaults% 6
.6 7 
AuthenticationScheme7 K
;K L
} 
) 
. 
AddJwtBearer 
( 
options 
=> 
{ 
options 
. %
TokenValidationParameters %
=& '
new( +%
TokenValidationParameters, E
{   
ValidateIssuer!! 
=!! 
true!! 
,!! 
ValidateAudience"" 
="" 
true"" 
,""  
ValidateLifetime## 
=## 
true## 
,##  $
ValidateIssuerSigningKey$$  
=$$! "
true$$# '
,$$' (
ValidIssuer%% 
=%% 
builder%% 
.%% 
Configuration%% +
[%%+ ,
$str%%, 8
]%%8 9
,%%9 :
ValidAudience&& 
=&& 
builder&& 
.&&  
Configuration&&  -
[&&- .
$str&&. <
]&&< =
,&&= >
IssuerSigningKey'' 
='' 
new''  
SymmetricSecurityKey'' 3
(''3 4
Encoding(( 
.(( 
UTF8(( 
.(( 
GetBytes(( "
(((" #
jwtKey((# )
)(() *
)((* +
})) 
;)) 
}** 
)** 
;** 
builder-- 
.-- 
Services-- 
.-- 
	AddScoped-- 
<-- 
ITemaRepository-- *
,--* +
TemaRepository--, :
>--: ;
(--; <
)--< =
;--= >
builder.. 
... 
Services.. 
... 
	AddScoped.. 
<.. 
IUsuarioRepository.. -
,..- .
UsuarioRepository../ @
>..@ A
(..A B
)..B C
;..C D
builder// 
.// 
Services// 
.// 
	AddScoped// 
<// 
IPostagemRepository// .
,//. /
PostagemRepository//0 B
>//B C
(//C D
)//D E
;//E F
builder22 
.22 
Services22 
.22 
	AddScoped22 
<22 
ITemaService22 '
,22' (
TemaService22) 4
>224 5
(225 6
)226 7
;227 8
builder33 
.33 
Services33 
.33 
	AddScoped33 
<33 
IUsuarioService33 *
,33* +
UsuarioService33, :
>33: ;
(33; <
)33< =
;33= >
builder44 
.44 
Services44 
.44 
	AddScoped44 
<44 
IPostagemService44 +
,44+ ,
PostagemService44- <
>44< =
(44= >
)44> ?
;44? @
builder55 
.55 
Services55 
.55 
	AddScoped55 
<55 

JwtService55 %
>55% &
(55& '
)55' (
;55( )
builder77 
.77 
Services77 
.77 
AddHttpClient77 
<77 

IIAService77 )
,77) *
GeminiService77+ 8
>778 9
(779 :
)77: ;
;77; <
builder:: 
.:: 
Services:: 
.:: 
AddControllers:: 
(::  
)::  !
.;; 
AddJsonOptions;; 
(;; 
options;; 
=>;; 
{<< 
options>> 
.>> !
JsonSerializerOptions>> %
.>>% &
ReferenceHandler>>& 6
=>>7 8
System?? 
.?? 
Text?? 
.?? 
Json?? 
.?? 
Serialization?? *
.??* +
ReferenceHandler??+ ;
.??; <
IgnoreCycles??< H
;??H I
}@@ 
)@@ 
;@@ 
builderAA 
.AA 
ServicesAA 
.AA #
AddEndpointsApiExplorerAA (
(AA( )
)AA) *
;AA* +
builderBB 
.BB 
ServicesBB 
.BB 

AddOpenApiBB 
(BB 
)BB 
;BB 
varDD 
appDD 
=DD 	
builderDD
 
.DD 
BuildDD 
(DD 
)DD 
;DD 
appGG 
.GG 
UseExceptionHandlerGG 
(GG 

appBuilderGG "
=>GG# %
{HH 

appBuilderII 
.II 
RunII 
(II 
asyncII 
contextII  
=>II! #
{JJ 
contextKK 
.KK 
ResponseKK 
.KK 

StatusCodeKK #
=KK$ %
$numKK& )
;KK) *
contextLL 
.LL 
ResponseLL 
.LL 
ContentTypeLL $
=LL% &
$strLL' 9
;LL9 :
awaitMM 
contextMM 
.MM 
ResponseMM 
.MM 
WriteAsJsonAsyncMM /
(MM/ 0
newNN 
{NN 
erroNN 
=NN 
$strNN 4
}NN5 6
)NN6 7
;NN7 8
}OO 
)OO 
;OO 
}PP 
)PP 
;PP 
ifSS 
(SS 
appSS 
.SS 
EnvironmentSS 
.SS 
IsDevelopmentSS !
(SS! "
)SS" #
)SS# $
{TT 
appUU 
.UU 

MapOpenApiUU 
(UU 
)UU 
;UU 
appVV 
.VV !
MapScalarApiReferenceVV 
(VV 
optionsVV %
=>VV& (
{WW 
optionsYY 
.YY !
AddHttpAuthenticationYY %
(YY% &
$strYY& .
,YY. /
authYY0 4
=>YY5 7
{ZZ 	
auth[[ 
.[[ 
Token[[ 
=[[ 
string[[ 
.[[  
Empty[[  %
;[[% &
}\\ 	
)\\	 

;\\
 
}]] 
)]] 
;]] 
}^^ 
app`` 
.`` 
UseHttpsRedirection`` 
(`` 
)`` 
;`` 
appaa 
.aa 
UseAuthenticationaa 
(aa 
)aa 
;aa 
appbb 
.bb 
UseAuthorizationbb 
(bb 
)bb 
;bb 
appcc 
.cc 
MapControllerscc 
(cc 
)cc 
;cc 
awaitee 
appee 	
.ee	 

RunAsyncee
 
(ee 
)ee 
;ee “	
oC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Models\UsuarioLogin.cs
	namespace 	
BlogPessoal
 
. 
Models 
; 
public 
class 
UsuarioLogin 
{ 
[ 
StringLength 
( 
$num 
, 
MinimumLength $
=% &
$num' (
)( )
]) *
[ 
EmailAddress 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
public 

string 
? 
Email 
{ 
get 
; 
set  #
;# $
}% &
[ 
Required 
( 
ErrorMessage 
= 
$str 5
)5 6
]6 7
[ 
Column 
( 
TypeName 
= 
$str %
)% &
]& '
public 

string 
? 
Senha 
{ 
get 
; 
set  #
;# $
}% &
} ”
jC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Models\Usuario.cs
	namespace 	
BlogPessoal
 
. 
Models 
; 
[ 
Table 
( 
$str 
) 
] 
public 
class 
Usuario 
{ 
[ 
Key 
] 	
[ 
DatabaseGenerated 
( #
DatabaseGeneratedOption .
.. /
Identity/ 7
)7 8
]8 9
public 

long 
Id 
{ 
get 
; 
set 
; 
}  
[ 
Required 
( 
ErrorMessage 
= 
$str 4
)4 5
]5 6
public 

string 
Nome 
{ 
get 
; 
set !
;! "
}# $
=% &
string' -
.- .
Empty. 3
;3 4
[ 
Required 
( 
ErrorMessage 
= 
$str 5
)5 6
]6 7
[ 
EmailAddress 
( 
ErrorMessage 
=  
$str! =
)= >
]> ?
public 

string 
Email 
{ 
get 
; 
set "
;" #
}$ %
=& '
string( .
.. /
Empty/ 4
;4 5
["" 
Required"" 
("" 
ErrorMessage"" 
="" 
$str"" 5
)""5 6
]""6 7
public## 

string## 
Senha## 
{## 
get## 
;## 
set## "
;##" #
}##$ %
=##& '
string##( .
.##. /
Empty##/ 4
;##4 5
[&& 
Column&& 
(&& 
TypeName&& 
=&& 
$str&& &
)&&& '
]&&' (
public'' 

string'' 
?'' 
Foto'' 
{'' 
get'' 
;'' 
set'' "
;''" #
}''$ %
[)) 

JsonIgnore)) 
()) 
	Condition)) 
=)) 
JsonIgnoreCondition)) /
.))/ 0
WhenWritingNull))0 ?
)))? @
]))@ A
public** 

virtual** 
ICollection** 
<** 
Postagem** '
>**' (
?**( )
Postagem*** 2
{**3 4
get**5 8
;**8 9
set**: =
;**= >
}**? @
}++ Ê
gC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Models\Tema.cs
	namespace 	
BlogPessoal
 
. 
Models 
; 
[ 
Table 
( 
$str 
) 
] 
public 
class 
Tema 
{ 
[ 
Key 
] 	
[ 
DatabaseGenerated 
( #
DatabaseGeneratedOption .
.. /
Identity/ 7
)7 8
]8 9
[ 
System 
. 
Text 
. 
Json 
. 
Serialization #
.# $
JsonRequired$ 0
]0 1
public 

long 
Id 
{ 
get 
; 
set 
; 
}  
[ 
Required 
( 
ErrorMessage 
= 
$str 9
)9 :
]: ;
public 

string 
	Descricao 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 

JsonIgnore 
( 
	Condition 
= 
JsonIgnoreCondition /
./ 0
WhenWritingNull0 ?
)? @
]@ A
public 

virtual 
ICollection 
< 
Postagem '
>' (
?( )
Postagem* 2
{3 4
get5 8
;8 9
set: =
;= >
}? @
} —
kC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Models\Postagem.cs
	namespace 	
BlogPessoal
 
. 
Models 
; 
[ 
Table 
( 
$str 
) 
] 
public 
class 
Postagem 
{ 
[ 
Key 
] 	
[ 
DatabaseGenerated 
( #
DatabaseGeneratedOption .
.. /
Identity/ 7
)7 8
]8 9
public 

long 
Id 
{ 
get 
; 
set 
; 
}  
[ 
Required 
( 
ErrorMessage 
= 
$str 6
)6 7
]7 8
[ 
StringLength 
( 
$num 
, 
MinimumLength $
=% &
$num' (
,( )
ErrorMessage* 6
=7 8
$str9 >
)> ?
]? @
public 

string 
Titulo 
{ 
get 
; 
set  #
;# $
}% &
=' (
string) /
./ 0
Empty0 5
;5 6
[ 
Required 
( 
ErrorMessage 
= 
$str 5
)5 6
]6 7
[ 
StringLength 
( 
$num 
, 
MinimumLength &
=' (
$num) +
,+ ,
ErrorMessage- 9
=: ;
$str< A
)A B
]B C
public 

string 
Texto 
{ 
get 
; 
set "
;" #
}$ %
=& '
string( .
.. /
Empty/ 4
;4 5
public"" 

DateTime"" 
?"" 
Data"" 
{"" 
get"" 
;""  
set""! $
;""$ %
}""& '
=""( )
DateTime""* 2
.""2 3
Now""3 6
;""6 7
public%% 

string%% 
?%% 
ResumoIA%% 
{%% 
get%% !
;%%! "
set%%# &
;%%& '
}%%( )
public&& 

string&& 
?&& 
TagsIA&& 
{&& 
get&& 
;&&  
set&&! $
;&&$ %
}&&& '
public'' 

string'' 
?'' 
CategoriaIA'' 
{''  
get''! $
;''$ %
set''& )
;'') *
}''+ ,
public** 

virtual** 
Tema** 
?** 
Tema** 
{** 
get**  #
;**# $
set**% (
;**( )
}*** +
public-- 

virtual-- 
Usuario-- 
?-- 
Usuario-- #
{--$ %
get--& )
;--) *
set--+ .
;--. /
}--0 1
}.. Ç}
áC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Migrations\20260522182942_AddRequiredFields.cs
	namespace 	
BlogPessoal
 
. 

Migrations  
{ 
public 

partial 
class 
AddRequiredFields *
:+ ,
	Migration- 6
{		 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 

UpdateData '
(' (
table 
: 
$str $
,$ %
	keyColumn 
: 
$str "
," #
keyValue 
: 
null 
, 
column 
: 
$str 
,  
value 
: 
$str 
) 
; 
migrationBuilder 
. 
AlterColumn (
<( )
string) /
>/ 0
(0 1
name 
: 
$str 
, 
table 
: 
$str $
,$ %
type 
: 
$str  
,  !
nullable 
: 
false 
,  

oldClrType 
: 
typeof "
(" #
string# )
)) *
,* +
oldType 
: 
$str '
,' (
oldMaxLength 
: 
$num !
,! "
oldNullable 
: 
true !
)! "
. 

Annotation 
( 
$str +
,+ ,
$str- 6
)6 7
. 
OldAnnotation 
( 
$str .
,. /
$str0 9
)9 :
;: ;
migrationBuilder   
.   

UpdateData   '
(  ' (
table!! 
:!! 
$str!! $
,!!$ %
	keyColumn"" 
:"" 
$str"" !
,""! "
keyValue## 
:## 
null## 
,## 
column$$ 
:$$ 
$str$$ 
,$$ 
value%% 
:%% 
$str%% 
)%% 
;%% 
migrationBuilder'' 
.'' 
AlterColumn'' (
<''( )
string'') /
>''/ 0
(''0 1
name(( 
:(( 
$str(( 
,(( 
table)) 
:)) 
$str)) $
,))$ %
type** 
:** 
$str**  
,**  !
nullable++ 
:++ 
false++ 
,++  

oldClrType,, 
:,, 
typeof,, "
(,," #
string,,# )
),,) *
,,,* +
oldType-- 
:-- 
$str-- '
,--' (
oldMaxLength.. 
:.. 
$num.. !
,..! "
oldNullable// 
:// 
true// !
)//! "
.00 

Annotation00 
(00 
$str00 +
,00+ ,
$str00- 6
)006 7
.11 
OldAnnotation11 
(11 
$str11 .
,11. /
$str110 9
)119 :
;11: ;
migrationBuilder33 
.33 

UpdateData33 '
(33' (
table44 
:44 
$str44 $
,44$ %
	keyColumn55 
:55 
$str55 "
,55" #
keyValue66 
:66 
null66 
,66 
column77 
:77 
$str77 
,77  
value88 
:88 
$str88 
)88 
;88 
migrationBuilder:: 
.:: 
AlterColumn:: (
<::( )
string::) /
>::/ 0
(::0 1
name;; 
:;; 
$str;; 
,;; 
table<< 
:<< 
$str<< $
,<<$ %
type== 
:== 
$str==  
,==  !
nullable>> 
:>> 
false>> 
,>>  

oldClrType?? 
:?? 
typeof?? "
(??" #
string??# )
)??) *
,??* +
oldType@@ 
:@@ 
$str@@ '
,@@' (
oldMaxLengthAA 
:AA 
$numAA !
,AA! "
oldNullableBB 
:BB 
trueBB !
)BB! "
.CC 

AnnotationCC 
(CC 
$strCC +
,CC+ ,
$strCC- 6
)CC6 7
.DD 
OldAnnotationDD 
(DD 
$strDD .
,DD. /
$strDD0 9
)DD9 :
;DD: ;
migrationBuilderFF 
.FF 

UpdateDataFF '
(FF' (
tableGG 
:GG 
$strGG !
,GG! "
	keyColumnHH 
:HH 
$strHH &
,HH& '
keyValueII 
:II 
nullII 
,II 
columnJJ 
:JJ 
$strJJ #
,JJ# $
valueKK 
:KK 
$strKK 
)KK 
;KK 
migrationBuilderMM 
.MM 
AlterColumnMM (
<MM( )
stringMM) /
>MM/ 0
(MM0 1
nameNN 
:NN 
$strNN !
,NN! "
tableOO 
:OO 
$strOO !
,OO! "
typePP 
:PP 
$strPP  
,PP  !
nullableQQ 
:QQ 
falseQQ 
,QQ  

oldClrTypeRR 
:RR 
typeofRR "
(RR" #
stringRR# )
)RR) *
,RR* +
oldTypeSS 
:SS 
$strSS '
,SS' (
oldMaxLengthTT 
:TT 
$numTT !
,TT! "
oldNullableUU 
:UU 
trueUU !
)UU! "
.VV 

AnnotationVV 
(VV 
$strVV +
,VV+ ,
$strVV- 6
)VV6 7
.WW 
OldAnnotationWW 
(WW 
$strWW .
,WW. /
$strWW0 9
)WW9 :
;WW: ;
migrationBuilderYY 
.YY 

UpdateDataYY '
(YY' (
tableZZ 
:ZZ 
$strZZ %
,ZZ% &
	keyColumn[[ 
:[[ 
$str[[ #
,[[# $
keyValue\\ 
:\\ 
null\\ 
,\\ 
column]] 
:]] 
$str]]  
,]]  !
value^^ 
:^^ 
$str^^ 
)^^ 
;^^ 
migrationBuilder`` 
.`` 
AlterColumn`` (
<``( )
string``) /
>``/ 0
(``0 1
nameaa 
:aa 
$straa 
,aa 
tablebb 
:bb 
$strbb %
,bb% &
typecc 
:cc 
$strcc $
,cc$ %
	maxLengthdd 
:dd 
$numdd 
,dd 
nullableee 
:ee 
falseee 
,ee  

oldClrTypeff 
:ff 
typeofff "
(ff" #
stringff# )
)ff) *
,ff* +
oldTypegg 
:gg 
$strgg '
,gg' (
oldMaxLengthhh 
:hh 
$numhh !
,hh! "
oldNullableii 
:ii 
trueii !
)ii! "
.jj 

Annotationjj 
(jj 
$strjj +
,jj+ ,
$strjj- 6
)jj6 7
.kk 
OldAnnotationkk 
(kk 
$strkk .
,kk. /
$strkk0 9
)kk9 :
;kk: ;
migrationBuildermm 
.mm 

UpdateDatamm '
(mm' (
tablenn 
:nn 
$strnn %
,nn% &
	keyColumnoo 
:oo 
$stroo "
,oo" #
keyValuepp 
:pp 
nullpp 
,pp 
columnqq 
:qq 
$strqq 
,qq  
valuerr 
:rr 
$strrr 
)rr 
;rr 
migrationBuildertt 
.tt 
AlterColumntt (
<tt( )
stringtt) /
>tt/ 0
(tt0 1
nameuu 
:uu 
$struu 
,uu 
tablevv 
:vv 
$strvv %
,vv% &
typeww 
:ww 
$strww &
,ww& '
	maxLengthxx 
:xx 
$numxx  
,xx  !
nullableyy 
:yy 
falseyy 
,yy  

oldClrTypezz 
:zz 
typeofzz "
(zz" #
stringzz# )
)zz) *
,zz* +
oldType{{ 
:{{ 
$str{{ )
,{{) *
oldMaxLength|| 
:|| 
$num|| #
,||# $
oldNullable}} 
:}} 
true}} !
)}}! "
.~~ 

Annotation~~ 
(~~ 
$str~~ +
,~~+ ,
$str~~- 6
)~~6 7
. 
OldAnnotation 
( 
$str .
,. /
$str0 9
)9 :
;: ;
}
ÄÄ 	
	protected
ÉÉ 
override
ÉÉ 
void
ÉÉ 
Down
ÉÉ  $
(
ÉÉ$ %
MigrationBuilder
ÉÉ% 5
migrationBuilder
ÉÉ6 F
)
ÉÉF G
{
ÑÑ 	
migrationBuilder
ÖÖ 
.
ÖÖ 
AlterColumn
ÖÖ (
<
ÖÖ( )
string
ÖÖ) /
>
ÖÖ/ 0
(
ÖÖ0 1
name
ÜÜ 
:
ÜÜ 
$str
ÜÜ 
,
ÜÜ 
table
áá 
:
áá 
$str
áá $
,
áá$ %
type
àà 
:
àà 
$str
àà $
,
àà$ %
	maxLength
ââ 
:
ââ 
$num
ââ 
,
ââ 
nullable
ää 
:
ää 
true
ää 
,
ää 

oldClrType
ãã 
:
ãã 
typeof
ãã "
(
ãã" #
string
ãã# )
)
ãã) *
,
ãã* +
oldType
åå 
:
åå 
$str
åå #
)
åå# $
.
çç 

Annotation
çç 
(
çç 
$str
çç +
,
çç+ ,
$str
çç- 6
)
çç6 7
.
éé 
OldAnnotation
éé 
(
éé 
$str
éé .
,
éé. /
$str
éé0 9
)
éé9 :
;
éé: ;
migrationBuilder
êê 
.
êê 
AlterColumn
êê (
<
êê( )
string
êê) /
>
êê/ 0
(
êê0 1
name
ëë 
:
ëë 
$str
ëë 
,
ëë 
table
íí 
:
íí 
$str
íí $
,
íí$ %
type
ìì 
:
ìì 
$str
ìì $
,
ìì$ %
	maxLength
îî 
:
îî 
$num
îî 
,
îî 
nullable
ïï 
:
ïï 
true
ïï 
,
ïï 

oldClrType
ññ 
:
ññ 
typeof
ññ "
(
ññ" #
string
ññ# )
)
ññ) *
,
ññ* +
oldType
óó 
:
óó 
$str
óó #
)
óó# $
.
òò 

Annotation
òò 
(
òò 
$str
òò +
,
òò+ ,
$str
òò- 6
)
òò6 7
.
ôô 
OldAnnotation
ôô 
(
ôô 
$str
ôô .
,
ôô. /
$str
ôô0 9
)
ôô9 :
;
ôô: ;
migrationBuilder
õõ 
.
õõ 
AlterColumn
õõ (
<
õõ( )
string
õõ) /
>
õõ/ 0
(
õõ0 1
name
úú 
:
úú 
$str
úú 
,
úú 
table
ùù 
:
ùù 
$str
ùù $
,
ùù$ %
type
ûû 
:
ûû 
$str
ûû $
,
ûû$ %
	maxLength
üü 
:
üü 
$num
üü 
,
üü 
nullable
†† 
:
†† 
true
†† 
,
†† 

oldClrType
°° 
:
°° 
typeof
°° "
(
°°" #
string
°°# )
)
°°) *
,
°°* +
oldType
¢¢ 
:
¢¢ 
$str
¢¢ #
)
¢¢# $
.
££ 

Annotation
££ 
(
££ 
$str
££ +
,
££+ ,
$str
££- 6
)
££6 7
.
§§ 
OldAnnotation
§§ 
(
§§ 
$str
§§ .
,
§§. /
$str
§§0 9
)
§§9 :
;
§§: ;
migrationBuilder
¶¶ 
.
¶¶ 
AlterColumn
¶¶ (
<
¶¶( )
string
¶¶) /
>
¶¶/ 0
(
¶¶0 1
name
ßß 
:
ßß 
$str
ßß !
,
ßß! "
table
®® 
:
®® 
$str
®® !
,
®®! "
type
©© 
:
©© 
$str
©© $
,
©©$ %
	maxLength
™™ 
:
™™ 
$num
™™ 
,
™™ 
nullable
´´ 
:
´´ 
true
´´ 
,
´´ 

oldClrType
¨¨ 
:
¨¨ 
typeof
¨¨ "
(
¨¨" #
string
¨¨# )
)
¨¨) *
,
¨¨* +
oldType
≠≠ 
:
≠≠ 
$str
≠≠ #
)
≠≠# $
.
ÆÆ 

Annotation
ÆÆ 
(
ÆÆ 
$str
ÆÆ +
,
ÆÆ+ ,
$str
ÆÆ- 6
)
ÆÆ6 7
.
ØØ 
OldAnnotation
ØØ 
(
ØØ 
$str
ØØ .
,
ØØ. /
$str
ØØ0 9
)
ØØ9 :
;
ØØ: ;
migrationBuilder
±± 
.
±± 
AlterColumn
±± (
<
±±( )
string
±±) /
>
±±/ 0
(
±±0 1
name
≤≤ 
:
≤≤ 
$str
≤≤ 
,
≤≤ 
table
≥≥ 
:
≥≥ 
$str
≥≥ %
,
≥≥% &
type
¥¥ 
:
¥¥ 
$str
¥¥ $
,
¥¥$ %
	maxLength
µµ 
:
µµ 
$num
µµ 
,
µµ 
nullable
∂∂ 
:
∂∂ 
true
∂∂ 
,
∂∂ 

oldClrType
∑∑ 
:
∑∑ 
typeof
∑∑ "
(
∑∑" #
string
∑∑# )
)
∑∑) *
,
∑∑* +
oldType
∏∏ 
:
∏∏ 
$str
∏∏ '
,
∏∏' (
oldMaxLength
ππ 
:
ππ 
$num
ππ !
)
ππ! "
.
∫∫ 

Annotation
∫∫ 
(
∫∫ 
$str
∫∫ +
,
∫∫+ ,
$str
∫∫- 6
)
∫∫6 7
.
ªª 
OldAnnotation
ªª 
(
ªª 
$str
ªª .
,
ªª. /
$str
ªª0 9
)
ªª9 :
;
ªª: ;
migrationBuilder
ΩΩ 
.
ΩΩ 
AlterColumn
ΩΩ (
<
ΩΩ( )
string
ΩΩ) /
>
ΩΩ/ 0
(
ΩΩ0 1
name
ææ 
:
ææ 
$str
ææ 
,
ææ 
table
øø 
:
øø 
$str
øø %
,
øø% &
type
¿¿ 
:
¿¿ 
$str
¿¿ &
,
¿¿& '
	maxLength
¡¡ 
:
¡¡ 
$num
¡¡  
,
¡¡  !
nullable
¬¬ 
:
¬¬ 
true
¬¬ 
,
¬¬ 

oldClrType
√√ 
:
√√ 
typeof
√√ "
(
√√" #
string
√√# )
)
√√) *
,
√√* +
oldType
ƒƒ 
:
ƒƒ 
$str
ƒƒ )
,
ƒƒ) *
oldMaxLength
≈≈ 
:
≈≈ 
$num
≈≈ #
)
≈≈# $
.
∆∆ 

Annotation
∆∆ 
(
∆∆ 
$str
∆∆ +
,
∆∆+ ,
$str
∆∆- 6
)
∆∆6 7
.
«« 
OldAnnotation
«« 
(
«« 
$str
«« .
,
««. /
$str
««0 9
)
««9 :
;
««: ;
}
»» 	
}
…… 
}   Ûa
ãC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Migrations\20260519112909_IniciandoBancoDeDados.cs
	namespace 	
BlogPessoal
 
. 

Migrations  
{ 
public

 

partial

 
class

 !
IniciandoBancoDeDados

 .
:

/ 0
	Migration

1 :
{ 
	protected 
override 
void 
Up  "
(" #
MigrationBuilder# 3
migrationBuilder4 D
)D E
{ 	
migrationBuilder 
. 
AlterDatabase *
(* +
)+ ,
. 

Annotation 
( 
$str +
,+ ,
$str- 6
)6 7
;7 8
migrationBuilder 
. 
CreateTable (
(( )
name 
: 
$str  
,  !
columns 
: 
table 
=> !
new" %
{ 
Id 
= 
table 
. 
Column %
<% &
long& *
>* +
(+ ,
type, 0
:0 1
$str2 :
,: ;
nullable< D
:D E
falseF K
)K L
. 

Annotation #
(# $
$str$ C
,C D(
MySqlValueGenerationStrategyE a
.a b
IdentityColumnb p
)p q
,q r
	Descricao 
= 
table  %
.% &
Column& ,
<, -
string- 3
>3 4
(4 5
type5 9
:9 :
$str; I
,I J
	maxLengthK T
:T U
$numV Y
,Y Z
nullable[ c
:c d
truee i
)i j
. 

Annotation #
(# $
$str$ 3
,3 4
$str5 >
)> ?
} 
, 
constraints 
: 
table "
=># %
{ 
table 
. 

PrimaryKey $
($ %
$str% 2
,2 3
x4 5
=>6 8
x9 :
.: ;
Id; =
)= >
;> ?
} 
) 
. 

Annotation 
( 
$str +
,+ ,
$str- 6
)6 7
;7 8
migrationBuilder!! 
.!! 
CreateTable!! (
(!!( )
name"" 
:"" 
$str"" #
,""# $
columns## 
:## 
table## 
=>## !
new##" %
{$$ 
Id%% 
=%% 
table%% 
.%% 
Column%% %
<%%% &
long%%& *
>%%* +
(%%+ ,
type%%, 0
:%%0 1
$str%%2 :
,%%: ;
nullable%%< D
:%%D E
false%%F K
)%%K L
.&& 

Annotation&& #
(&&# $
$str&&$ C
,&&C D(
MySqlValueGenerationStrategy&&E a
.&&a b
IdentityColumn&&b p
)&&p q
,&&q r
Nome'' 
='' 
table''  
.''  !
Column''! '
<''' (
string''( .
>''. /
(''/ 0
type''0 4
:''4 5
$str''6 D
,''D E
	maxLength''F O
:''O P
$num''Q T
,''T U
nullable''V ^
:''^ _
true''` d
)''d e
.(( 

Annotation(( #
(((# $
$str(($ 3
,((3 4
$str((5 >
)((> ?
,((? @
Email)) 
=)) 
table)) !
.))! "
Column))" (
<))( )
string))) /
>))/ 0
())0 1
type))1 5
:))5 6
$str))7 E
,))E F
	maxLength))G P
:))P Q
$num))R U
,))U V
nullable))W _
:))_ `
true))a e
)))e f
.** 

Annotation** #
(**# $
$str**$ 3
,**3 4
$str**5 >
)**> ?
,**? @
Senha++ 
=++ 
table++ !
.++! "
Column++" (
<++( )
string++) /
>++/ 0
(++0 1
type++1 5
:++5 6
$str++7 E
,++E F
	maxLength++G P
:++P Q
$num++R U
,++U V
nullable++W _
:++_ `
true++a e
)++e f
.,, 

Annotation,, #
(,,# $
$str,,$ 3
,,,3 4
$str,,5 >
),,> ?
,,,? @
Foto-- 
=-- 
table--  
.--  !
Column--! '
<--' (
string--( .
>--. /
(--/ 0
type--0 4
:--4 5
$str--6 E
,--E F
nullable--G O
:--O P
true--Q U
)--U V
... 

Annotation.. #
(..# $
$str..$ 3
,..3 4
$str..5 >
)..> ?
}// 
,// 
constraints00 
:00 
table00 "
=>00# %
{11 
table22 
.22 

PrimaryKey22 $
(22$ %
$str22% 5
,225 6
x227 8
=>229 ;
x22< =
.22= >
Id22> @
)22@ A
;22A B
}33 
)33 
.44 

Annotation44 
(44 
$str44 +
,44+ ,
$str44- 6
)446 7
;447 8
migrationBuilder66 
.66 
CreateTable66 (
(66( )
name77 
:77 
$str77 $
,77$ %
columns88 
:88 
table88 
=>88 !
new88" %
{99 
Id:: 
=:: 
table:: 
.:: 
Column:: %
<::% &
long::& *
>::* +
(::+ ,
type::, 0
:::0 1
$str::2 :
,::: ;
nullable::< D
:::D E
false::F K
)::K L
.;; 

Annotation;; #
(;;# $
$str;;$ C
,;;C D(
MySqlValueGenerationStrategy;;E a
.;;a b
IdentityColumn;;b p
);;p q
,;;q r
Titulo<< 
=<< 
table<< "
.<<" #
Column<<# )
<<<) *
string<<* 0
><<0 1
(<<1 2
type<<2 6
:<<6 7
$str<<8 F
,<<F G
	maxLength<<H Q
:<<Q R
$num<<S V
,<<V W
nullable<<X `
:<<` a
true<<b f
)<<f g
.== 

Annotation== #
(==# $
$str==$ 3
,==3 4
$str==5 >
)==> ?
,==? @
Texto>> 
=>> 
table>> !
.>>! "
Column>>" (
<>>( )
string>>) /
>>>/ 0
(>>0 1
type>>1 5
:>>5 6
$str>>7 G
,>>G H
	maxLength>>I R
:>>R S
$num>>T Y
,>>Y Z
nullable>>[ c
:>>c d
true>>e i
)>>i j
.?? 

Annotation?? #
(??# $
$str??$ 3
,??3 4
$str??5 >
)??> ?
,??? @
Data@@ 
=@@ 
table@@  
.@@  !
Column@@! '
<@@' (
DateTime@@( 0
>@@0 1
(@@1 2
type@@2 6
:@@6 7
$str@@8 E
,@@E F
nullable@@G O
:@@O P
true@@Q U
)@@U V
,@@V W
ResumoIAAA 
=AA 
tableAA $
.AA$ %
ColumnAA% +
<AA+ ,
stringAA, 2
>AA2 3
(AA3 4
typeAA4 8
:AA8 9
$strAA: D
,AAD E
nullableAAF N
:AAN O
trueAAP T
)AAT U
.BB 

AnnotationBB #
(BB# $
$strBB$ 3
,BB3 4
$strBB5 >
)BB> ?
,BB? @
TagsIACC 
=CC 
tableCC "
.CC" #
ColumnCC# )
<CC) *
stringCC* 0
>CC0 1
(CC1 2
typeCC2 6
:CC6 7
$strCC8 B
,CCB C
nullableCCD L
:CCL M
trueCCN R
)CCR S
.DD 

AnnotationDD #
(DD# $
$strDD$ 3
,DD3 4
$strDD5 >
)DD> ?
,DD? @
CategoriaIAEE 
=EE  !
tableEE" '
.EE' (
ColumnEE( .
<EE. /
stringEE/ 5
>EE5 6
(EE6 7
typeEE7 ;
:EE; <
$strEE= G
,EEG H
nullableEEI Q
:EEQ R
trueEES W
)EEW X
.FF 

AnnotationFF #
(FF# $
$strFF$ 3
,FF3 4
$strFF5 >
)FF> ?
,FF? @
TemaIdGG 
=GG 
tableGG "
.GG" #
ColumnGG# )
<GG) *
longGG* .
>GG. /
(GG/ 0
typeGG0 4
:GG4 5
$strGG6 >
,GG> ?
nullableGG@ H
:GGH I
trueGGJ N
)GGN O
,GGO P
	UsuarioIdHH 
=HH 
tableHH  %
.HH% &
ColumnHH& ,
<HH, -
longHH- 1
>HH1 2
(HH2 3
typeHH3 7
:HH7 8
$strHH9 A
,HHA B
nullableHHC K
:HHK L
trueHHM Q
)HHQ R
}II 
,II 
constraintsJJ 
:JJ 
tableJJ "
=>JJ# %
{KK 
tableLL 
.LL 

PrimaryKeyLL $
(LL$ %
$strLL% 6
,LL6 7
xLL8 9
=>LL: <
xLL= >
.LL> ?
IdLL? A
)LLA B
;LLB C
tableMM 
.MM 

ForeignKeyMM $
(MM$ %
nameNN 
:NN 
$strNN ?
,NN? @
columnOO 
:OO 
xOO  !
=>OO" $
xOO% &
.OO& '
TemaIdOO' -
,OO- .
principalTablePP &
:PP& '
$strPP( 2
,PP2 3
principalColumnQQ '
:QQ' (
$strQQ) -
,QQ- .
onDeleteRR  
:RR  !
ReferentialActionRR" 3
.RR3 4
RestrictRR4 <
)RR< =
;RR= >
tableSS 
.SS 

ForeignKeySS $
(SS$ %
nameTT 
:TT 
$strTT E
,TTE F
columnUU 
:UU 
xUU  !
=>UU" $
xUU% &
.UU& '
	UsuarioIdUU' 0
,UU0 1
principalTableVV &
:VV& '
$strVV( 5
,VV5 6
principalColumnWW '
:WW' (
$strWW) -
,WW- .
onDeleteXX  
:XX  !
ReferentialActionXX" 3
.XX3 4
RestrictXX4 <
)XX< =
;XX= >
}YY 
)YY 
.ZZ 

AnnotationZZ 
(ZZ 
$strZZ +
,ZZ+ ,
$strZZ- 6
)ZZ6 7
;ZZ7 8
migrationBuilder\\ 
.\\ 
CreateIndex\\ (
(\\( )
name]] 
:]] 
$str]] .
,]]. /
table^^ 
:^^ 
$str^^ %
,^^% &
column__ 
:__ 
$str__  
)__  !
;__! "
migrationBuilderaa 
.aa 
CreateIndexaa (
(aa( )
namebb 
:bb 
$strbb 1
,bb1 2
tablecc 
:cc 
$strcc %
,cc% &
columndd 
:dd 
$strdd #
)dd# $
;dd$ %
}ee 	
	protectedhh 
overridehh 
voidhh 
Downhh  $
(hh$ %
MigrationBuilderhh% 5
migrationBuilderhh6 F
)hhF G
{ii 	
migrationBuilderjj 
.jj 
	DropTablejj &
(jj& '
namekk 
:kk 
$strkk $
)kk$ %
;kk% &
migrationBuildermm 
.mm 
	DropTablemm &
(mm& '
namenn 
:nn 
$strnn  
)nn  !
;nn! "
migrationBuilderpp 
.pp 
	DropTablepp &
(pp& '
nameqq 
:qq 
$strqq #
)qq# $
;qq$ %
}rr 	
}ss 
}tt º
sC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\DTOs\UsuarioResponseDTO.cs
	namespace 	
BlogPessoal
 
. 
DTOs 
; 
public 
class 
UsuarioResponseDTO 
{ 
public 

long 
Id 
{ 
get 
; 
set 
; 
}  
public 

string 
? 
Nome 
{ 
get 
; 
set "
;" #
}$ %
public 

string 
? 
Email 
{ 
get 
; 
set  #
;# $
}% &
public 

string 
? 
Foto 
{ 
get 
; 
set "
;" #
}$ %
}		 ∆
oC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\DTOs\UsuarioRequest.cs
	namespace 	
BlogPessoal
 
. 
DTOs 
{ 
public 

class 
UsuarioRequestDTO "
{ 
[ 	
Required	 
] 
public		 
string		 
Nome		 
{		 
get		  
;		  !
set		" %
;		% &
}		' (
=		) *
string		+ 1
.		1 2
Empty		2 7
;		7 8
[ 	
Required	 
, 
EmailAddress 
]  
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
[ 	
Required	 
, 
	MinLength 
( 
$num 
) 
]  
public 
string 
Senha 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
public 
string 
? 
Foto 
{ 
get !
;! "
set# &
;& '
}( )
} 
} Ô
lC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\DTOs\ResultadoIA.cs
	namespace 	
BlogPessoal
 
. 
DTOs 
; 
public 
class 
ResultadoIA 
{ 
public		 

string		 
Resumo		 
{		 
get		 
;		 
set		  #
;		# $
}		% &
=		' (
string		) /
.		/ 0
Empty		0 5
;		5 6
public 

string 
Tags 
{ 
get 
; 
set !
;! "
}# $
=% &
string' -
.- .
Empty. 3
;3 4
public 

string 
	Categoria 
{ 
get !
;! "
set# &
;& '
}( )
=* +
string, 2
.2 3
Empty3 8
;8 9
} ß
tC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\DTOs\PostagemResponseDTO.cs
	namespace 	
BlogPessoal
 
. 
DTOs 
; 
public 
class 
PostagemResponseDTO  
{ 
public		 

long		 
Id		 
{		 
get		 
;		 
set		 
;		 
}		  
public

 

string

 
?

 
Titulo

 
{

 
get

 
;

  
set

! $
;

$ %
}

& '
public 

string 
? 
Texto 
{ 
get 
; 
set  #
;# $
}% &
public 

DateTime 
? 
Data 
{ 
get 
;  
set! $
;$ %
}& '
public 

string 
? 
ResumoIA 
{ 
get !
;! "
set# &
;& '
}( )
public 

string 
? 
TagsIA 
{ 
get 
;  
set! $
;$ %
}& '
public 

string 
? 
CategoriaIA 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 

TemaResumoDTO 
? 
Tema 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 

UsuarioResumoDTO 
? 
Usuario $
{% &
get' *
;* +
set, /
;/ 0
}1 2
} 
public 
class 
TemaResumoDTO 
{ 
public 

long 
Id 
{ 
get 
; 
set 
; 
}  
public 

string 
? 
	Descricao 
{ 
get "
;" #
set$ '
;' (
}) *
} 
public 
class 
UsuarioResumoDTO 
{ 
public   

long   
Id   
{   
get   
;   
set   
;   
}    
public!! 

string!! 
?!! 
Nome!! 
{!! 
get!! 
;!! 
set!! "
;!!" #
}!!$ %
public"" 

string"" 
?"" 
Email"" 
{"" 
get"" 
;"" 
set""  #
;""# $
}""% &
public## 

string## 
?## 
Foto## 
{## 
get## 
;## 
set## "
;##" #
}##$ %
}$$ é
sC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\DTOs\PostagemRequestDTO.cs
	namespace 	
BlogPessoal
 
. 
DTOs 
; 
public 
class 
PostagemRequestDTO 
{ 
[ 
Required 
, 
StringLength 
( 
$num 
,  
MinimumLength! .
=/ 0
$num1 2
)2 3
]3 4
public 

string 
Titulo 
{ 
get 
; 
set  #
;# $
}% &
=' (
string) /
./ 0
Empty0 5
;5 6
[

 
Required

 
,

 
StringLength

 
(

 
$num

 !
,

! "
MinimumLength

# 0
=

1 2
$num

3 5
)

5 6
]

6 7
public 

string 
Texto 
{ 
get 
; 
set "
;" #
}$ %
=& '
string( .
.. /
Empty/ 4
;4 5
public 

long 
? 
TemaId 
{ 
get 
; 
set "
;" #
}$ %
public 

long 
? 
	UsuarioId 
{ 
get  
;  !
set" %
;% &
}' (
} ∞
mC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Data\AppDbContext.cs
	namespace 	
BlogPessoal
 
. 
Data 
; 
public 
class 
AppDbContext 
: 
	DbContext %
{ 
public 

AppDbContext 
( 
DbContextOptions (
<( )
AppDbContext) 5
>5 6
options7 >
)> ?
:@ A
baseB F
(F G
optionsG N
)N O
{P Q
}R S
public 

DbSet 
< 
Tema 
> 
Temas 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
public 

DbSet 
< 
Usuario 
> 
Usuarios "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
null3 7
!7 8
;8 9
public 

DbSet 
< 
Postagem 
> 
	Postagens $
{% &
get' *
;* +
set, /
;/ 0
}1 2
=3 4
null5 9
!9 :
;: ;
	protected 
override 
void 
OnModelCreating +
(+ ,
ModelBuilder, 8
modelBuilder9 E
)E F
{ 
base 
. 
OnModelCreating 
( 
modelBuilder )
)) *
;* +
modelBuilder   
.   
Entity   
<   
Postagem   $
>  $ %
(  % &
)  & '
.!! 
HasOne!! 
(!! 
p!! 
=>!! 
p!! 
.!! 
Tema!! 
)!!  
."" 
WithMany"" 
("" 
t"" 
=>"" 
t"" 
."" 
Postagem"" %
)""% &
.## 
HasForeignKey## 
(## 
$str## #
)### $
.$$ 
OnDelete$$ 
($$ 
DeleteBehavior$$ $
.$$$ %
Restrict$$% -
)$$- .
;$$. /
modelBuilder'' 
.'' 
Entity'' 
<'' 
Postagem'' $
>''$ %
(''% &
)''& '
.(( 
HasOne(( 
((( 
p(( 
=>(( 
p(( 
.(( 
Usuario(( "
)((" #
.)) 
WithMany)) 
()) 
u)) 
=>)) 
u)) 
.)) 
Postagem)) %
)))% &
.** 
HasForeignKey** 
(** 
$str** &
)**& '
.++ 
OnDelete++ 
(++ 
DeleteBehavior++ $
.++$ %
Restrict++% -
)++- .
;++. /
},, 
}-- ⁄O
yC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Controllers\UsuarioController.cs
	namespace 	
BlogPessoal
 
. 
Controllers !
;! "
[ 
	Authorize 

]
 
[ 
ApiController 
] 
[ 
Route 
( 
$str 
) 
] 
public 
class 
UsuarioController 
:  
ControllerBase! /
{ 
private 
readonly 
IUsuarioService $
_usuarioService% 4
;4 5
public 

UsuarioController 
( 
IUsuarioService ,
usuarioService- ;
); <
{ 
_usuarioService 
= 
usuarioService (
;( )
} 
private 
static 
UsuarioResponseDTO %
MapToDTO& .
(. /
Usuario/ 6
u7 8
)8 9
=>: <
new= @
(@ A
)A B
{ 
Id 

= 
u 
. 
Id 
, 
Nome   
=   
u   
.   
Nome   
,   
Email!! 
=!! 
u!! 
.!! 
Email!! 
,!! 
Foto"" 
="" 
u"" 
."" 
Foto"" 
}## 
;## 
['' 
HttpGet'' 
]'' 
public(( 

async(( 
Task(( 
<(( 
IActionResult(( #
>((# $
GetAll((% +
(((+ ,
)((, -
{)) 
var++ 
usuarios++ 
=++ 
await++ 
_usuarioService++ ,
.++, -
GetAllAsync++- 8
(++8 9
)++9 :
;++: ;
return-- 
Ok-- 
(-- 
usuarios-- 
.-- 
Select-- !
(--! "
MapToDTO--" *
)--* +
)--+ ,
;--, -
}.. 
[22 
HttpGet22 
(22 
$str22 
)22 
]22 
public33 

async33 
Task33 
<33 
IActionResult33 #
>33# $
GetById33% ,
(33, -
long33- 1
id332 4
)334 5
{44 
var66 
usuario66 
=66 
await66 
_usuarioService66 +
.66+ ,
GetByIdAsync66, 8
(668 9
id669 ;
)66; <
;66< =
if99 

(99 
usuario99 
is99 
null99 
)99 
return99 #
NotFound99$ ,
(99, -
)99- .
;99. /
return<< 
Ok<< 
(<< 
MapToDTO<< 
(<< 
usuario<< "
)<<" #
)<<# $
;<<$ %
}== 
[AA 
HttpPostAA 
(AA 
$strAA 
)AA 
]AA 
[BB 
AllowAnonymousBB 
]BB 
publicCC 

asyncCC 
TaskCC 
<CC 
IActionResultCC #
>CC# $
CreateCC% +
(CC+ ,
[CC, -
FromBodyCC- 5
]CC5 6
UsuarioRequestDTOCC7 H
dtoCCI L
)CCL M
{DD 
ifEE 

(EE 
!EE 

ModelStateEE 
.EE 
IsValidEE 
)EE  
returnEE! '

BadRequestEE( 2
(EE2 3

ModelStateEE3 =
)EE= >
;EE> ?
varGG 
usuarioGG 
=GG 
newGG 
UsuarioGG !
{HH 	
NomeII 
=II 
dtoII 
.II 
NomeII 
,II 
EmailJJ 
=JJ 
dtoJJ 
.JJ 
EmailJJ 
,JJ 
SenhaKK 
=KK 
dtoKK 
.KK 
SenhaKK 
,KK 
FotoLL 
=LL 
dtoLL 
.LL 
FotoLL 
}MM 	
;MM	 

varOO 
createdOO 
=OO 
awaitOO 
_usuarioServiceOO +
.OO+ ,
CreateAsyncOO, 7
(OO7 8
usuarioOO8 ?
)OO? @
;OO@ A
ifPP 

(PP 
createdPP 
isPP 
nullPP 
)PP 
returnPP #
ConflictPP$ ,
(PP, -
$strPP- C
)PPC D
;PPD E
returnRR 
CreatedAtActionRR 
(RR 
nameofRR %
(RR% &
GetByIdRR& -
)RR- .
,RR. /
newRR0 3
{RR4 5
idRR6 8
=RR9 :
createdRR; B
.RRB C
IdRRC E
}RRF G
,RRG H
MapToDTORRI Q
(RRQ R
createdRRR Y
)RRY Z
)RRZ [
;RR[ \
}SS 
[WW 
HttpPutWW 
(WW 
$strWW 
)WW 
]WW 
publicXX 

asyncXX 
TaskXX 
<XX 
IActionResultXX #
>XX# $
UpdateXX% +
(XX+ ,
longXX, 0
idXX1 3
,XX3 4
[XX5 6
FromBodyXX6 >
]XX> ?
UsuarioRequestDTOXX@ Q
dtoXXR U
)XXU V
{YY 
var[[ 
userId[[ 
=[[ 
long[[ 
.[[ 
Parse[[ 
([[  
User[[  $
.[[$ %
	FindFirst[[% .
([[. /

ClaimTypes[[/ 9
.[[9 :
NameIdentifier[[: H
)[[H I
?[[I J
.[[J K
Value[[K P
??[[Q S
$str[[T W
)[[W X
;[[X Y
if\\ 

(\\ 
userId\\ 
!=\\ 
id\\ 
)\\ 
return\\  
Forbid\\! '
(\\' (
)\\( )
;\\) *
if^^ 

(^^ 
!^^ 

ModelState^^ 
.^^ 
IsValid^^ 
)^^  
return^^! '

BadRequest^^( 2
(^^2 3

ModelState^^3 =
)^^= >
;^^> ?
var`` 
usuario`` 
=`` 
new`` 
Usuario`` !
{aa 	
Idbb 
=bb 
idbb 
,bb 
Nomecc 
=cc 
dtocc 
.cc 
Nomecc 
,cc 
Emaildd 
=dd 
dtodd 
.dd 
Emaildd 
,dd 
Senhaee 
=ee 
dtoee 
.ee 
Senhaee 
,ee 
Fotoff 
=ff 
dtoff 
.ff 
Fotoff 
}gg 	
;gg	 

varii 
updatedii 
=ii 
awaitii 
_usuarioServiceii +
.ii+ ,
UpdateAsyncii, 7
(ii7 8
usuarioii8 ?
)ii? @
;ii@ A
ifjj 

(jj 
updatedjj 
isjj 
nulljj 
)jj 
returnjj #
NotFoundjj$ ,
(jj, -
)jj- .
;jj. /
returnll 
Okll 
(ll 
MapToDTOll 
(ll 
updatedll "
)ll" #
)ll# $
;ll$ %
}mm 
[qq 

HttpDeleteqq 
(qq 
$strqq 
)qq 
]qq 
publicrr 

asyncrr 
Taskrr 
<rr 
IActionResultrr #
>rr# $
Deleterr% +
(rr+ ,
longrr, 0
idrr1 3
)rr3 4
{ss 
varuu 
userIduu 
=uu 
longuu 
.uu 
Parseuu 
(uu  
Useruu  $
.uu$ %
	FindFirstuu% .
(uu. /

ClaimTypesuu/ 9
.uu9 :
NameIdentifieruu: H
)uuH I
?uuI J
.uuJ K
ValueuuK P
??uuQ S
$struuT W
)uuW X
;uuX Y
ifvv 

(vv 
userIdvv 
!=vv 
idvv 
)vv 
returnvv  
Forbidvv! '
(vv' (
)vv( )
;vv) *
varxx 
deletedxx 
=xx 
awaitxx 
_usuarioServicexx +
.xx+ ,
DeleteAsyncxx, 7
(xx7 8
idxx8 :
)xx: ;
;xx; <
ifyy 

(yy 
!yy 
deletedyy 
)yy 
returnyy 
NotFoundyy %
(yy% &
)yy& '
;yy' (
return{{ 
	NoContent{{ 
({{ 
){{ 
;{{ 
}|| 
[
ÄÄ 
HttpPost
ÄÄ 
(
ÄÄ 
$str
ÄÄ 
)
ÄÄ 
]
ÄÄ 
[
ÅÅ 
AllowAnonymous
ÅÅ 
]
ÅÅ 
public
ÇÇ 

async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 
IActionResult
ÇÇ #
>
ÇÇ# $
Login
ÇÇ% *
(
ÇÇ* +
[
ÇÇ+ ,
FromBody
ÇÇ, 4
]
ÇÇ4 5
UsuarioLogin
ÇÇ6 B
usuarioLogin
ÇÇC O
)
ÇÇO P
{
ÉÉ 
if
ÖÖ 

(
ÖÖ 
!
ÖÖ 

ModelState
ÖÖ 
.
ÖÖ 
IsValid
ÖÖ 
)
ÖÖ  
return
ÖÖ! '

BadRequest
ÖÖ( 2
(
ÖÖ2 3

ModelState
ÖÖ3 =
)
ÖÖ= >
;
ÖÖ> ?
var
àà 
token
àà 
=
àà 
await
àà 
_usuarioService
àà )
.
àà) *

LoginAsync
àà* 4
(
àà4 5
usuarioLogin
àà5 A
)
ààA B
;
ààB C
if
ãã 

(
ãã 
token
ãã 
is
ãã 
null
ãã 
)
ãã 
return
ãã !
Unauthorized
ãã" .
(
ãã. /
$str
ãã/ J
)
ããJ K
;
ããK L
return
éé 
Ok
éé 
(
éé 
new
éé 
{
éé 
token
éé 
}
éé 
)
éé  
;
éé  !
}
èè 
}êê ê*
vC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Controllers\TemaController.cs
	namespace

 	
BlogPessoal


 
.

 
Controllers

 !
;

! "
[ 
	Authorize 

]
 
[ 
ApiController 
] 
[ 
Route 
( 
$str 
) 
] 
public 
class 
TemaController 
: 
ControllerBase ,
{ 
private 
readonly 
ITemaService !
_temaService" .
;. /
public 

TemaController 
( 
ITemaService &
temaService' 2
)2 3
{ 
_temaService 
= 
temaService "
;" #
} 
[ 
HttpGet 
] 
public 

async 
Task 
< 
IActionResult #
># $
GetAll% +
(+ ,
), -
{ 
var 
temas 
= 
await 
_temaService &
.& '
GetAllAsync' 2
(2 3
)3 4
;4 5
return!! 
Ok!! 
(!! 
temas!! 
)!! 
;!! 
}"" 
[%% 
HttpGet%% 
(%% 
$str%% 
)%% 
]%% 
public&& 

async&& 
Task&& 
<&& 
IActionResult&& #
>&&# $
GetById&&% ,
(&&, -
long&&- 1
id&&2 4
)&&4 5
{'' 
var)) 
tema)) 
=)) 
await)) 
_temaService)) %
.))% &
GetByIdAsync))& 2
())2 3
id))3 5
)))5 6
;))6 7
if,, 

(,, 
tema,, 
is,, 
null,, 
),, 
return,,  
NotFound,,! )
(,,) *
),,* +
;,,+ ,
return// 
Ok// 
(// 
tema// 
)// 
;// 
}00 
[33 
HttpPost33 
]33 
public44 

async44 
Task44 
<44 
IActionResult44 #
>44# $
Create44% +
(44+ ,
[44, -
FromBody44- 5
]445 6
Tema447 ;
tema44< @
)44@ A
{55 
if77 

(77 
!77 

ModelState77 
.77 
IsValid77 
)77  
return77! '

BadRequest77( 2
(772 3

ModelState773 =
)77= >
;77> ?
var:: 
created:: 
=:: 
await:: 
_temaService:: (
.::( )
CreateAsync::) 4
(::4 5
tema::5 9
)::9 :
;::: ;
return== 
CreatedAtAction== 
(== 
nameof>> "
(>>" #
GetById>># *
)>>* +
,>>+ ,
new?? 
{??  !
id??" $
=??% &
created??' .
.??. /
Id??/ 1
}??2 3
,??3 4
created@@ #
)AA 
;AA 
}BB 
[EE 
HttpPutEE 
(EE 
$strEE 
)EE 
]EE 
publicFF 

asyncFF 
TaskFF 
<FF 
IActionResultFF #
>FF# $
UpdateFF% +
(FF+ ,
longFF, 0
idFF1 3
,FF3 4
[FF5 6
FromBodyFF6 >
]FF> ?
TemaFF@ D
temaFFE I
)FFI J
{GG 
ifHH 

(HH 
!HH 

ModelStateHH 
.HH 
IsValidHH 
)HH  
returnHH! '

BadRequestHH( 2
(HH2 3

ModelStateHH3 =
)HH= >
;HH> ?
temaKK 
.KK 
IdKK 
=KK 
idKK 
;KK 
varNN 
updatedNN 
=NN 
awaitNN 
_temaServiceNN (
.NN( )
UpdateAsyncNN) 4
(NN4 5
temaNN5 9
)NN9 :
;NN: ;
ifQQ 

(QQ 
updatedQQ 
isQQ 
nullQQ 
)QQ 
returnQQ #
NotFoundQQ$ ,
(QQ, -
)QQ- .
;QQ. /
returnTT 
OkTT 
(TT 
updatedTT 
)TT 
;TT 
}UU 
[XX 

HttpDeleteXX 
(XX 
$strXX 
)XX 
]XX 
publicYY 

asyncYY 
TaskYY 
<YY 
IActionResultYY #
>YY# $
DeleteYY% +
(YY+ ,
longYY, 0
idYY1 3
)YY3 4
{ZZ 
var\\ 
deleted\\ 
=\\ 
await\\ 
_temaService\\ (
.\\( )
DeleteAsync\\) 4
(\\4 5
id\\5 7
)\\7 8
;\\8 9
if__ 

(__ 
!__ 
deleted__ 
)__ 
return__ 
NotFound__ %
(__% &
)__& '
;__' (
returnbb 
	NoContentbb 
(bb 
)bb 
;bb 
}cc 
}dd ¶e
zC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Controllers\PostagemController.cs
	namespace 	
BlogPessoal
 
. 
Controllers !
;! "
[ 
	Authorize 

]
 
[ 
ApiController 
] 
[ 
Route 
( 
$str 
) 
] 
public 
class 
PostagemController 
:  !
ControllerBase" 0
{ 
private 
readonly 
IPostagemService %
_postagemService& 6
;6 7
private 
readonly 
ITemaService !
_temaService" .
;. /
private 
readonly 
IUsuarioService $
_usuarioService% 4
;4 5
public 

PostagemController 
( 
IPostagemService 
postagemService (
,( )
ITemaService 
temaService  
,  !
IUsuarioService 
usuarioService &
)& '
{ 
_postagemService 
= 
postagemService +
;+ ,
_temaService 
= 
temaService '
;' (
_usuarioService 
= 
usuarioService *
;* +
} 
private!! 
static!! 
PostagemResponseDTO!! &
MapToDTO!!' /
(!!/ 0
Postagem!!0 8
p!!9 :
)!!: ;
=>!!< >
new!!? B
(!!B C
)!!C D
{"" 
Id## 

=## 
p## 
.## 
Id## 
,## 
Titulo$$ 
=$$ 
p$$ 
.$$ 
Titulo$$ 
,$$ 
Texto%% 
=%% 
p%% 
.%% 
Texto%% 
,%% 
Data&& 
=&& 
p&& 
.&& 
Data&& 
,&& 
ResumoIA'' 
='' 
p'' 
.'' 
ResumoIA'' 
,'' 
TagsIA(( 
=(( 
p(( 
.(( 
TagsIA(( 
,(( 
CategoriaIA)) 
=)) 
p)) 
.)) 
CategoriaIA)) #
,))# $
Tema** 
=** 
p** 
.** 
Tema** 
is** 
null** 
?** 
null**  $
:**% &
new**' *
TemaResumoDTO**+ 8
{++ 	
Id,, 
=,, 
p,, 
.,, 
Tema,, 
.,, 
Id,, 
,,, 
	Descricao-- 
=-- 
p-- 
.-- 
Tema-- 
.-- 
	Descricao-- (
}.. 	
,..	 

Usuario// 
=// 
p// 
.// 
Usuario// 
is// 
null// #
?//$ %
null//& *
://+ ,
new//- 0
UsuarioResumoDTO//1 A
{00 	
Id11 
=11 
p11 
.11 
Usuario11 
.11 
Id11 
,11 
Nome22 
=22 
p22 
.22 
Usuario22 
.22 
Nome22 !
,22! "
Email33 
=33 
p33 
.33 
Usuario33 
.33 
Email33 #
,33# $
Foto44 
=44 
p44 
.44 
Usuario44 
.44 
Foto44 !
}55 	
}66 
;66 
[99 
HttpGet99 
]99 
public:: 

async:: 
Task:: 
<:: 
IActionResult:: #
>::# $
GetAll::% +
(::+ ,
)::, -
{;; 
var<< 
	postagens<< 
=<< 
await<< 
_postagemService<< .
.<<. /
GetAllAsync<</ :
(<<: ;
)<<; <
;<<< =
return>> 
Ok>> 
(>> 
	postagens>> 
.>> 
Select>> "
(>>" #
MapToDTO>># +
)>>+ ,
)>>, -
;>>- .
}?? 
[BB 
HttpGetBB 
(BB 
$strBB 
)BB 
]BB 
publicCC 

asyncCC 
TaskCC 
<CC 
IActionResultCC #
>CC# $
GetByIdCC% ,
(CC, -
longCC- 1
idCC2 4
)CC4 5
{DD 
varEE 
postagemEE 
=EE 
awaitEE 
_postagemServiceEE -
.EE- .
GetByIdAsyncEE. :
(EE: ;
idEE; =
)EE= >
;EE> ?
ifFF 

(FF 
postagemFF 
isFF 
nullFF 
)FF 
returnFF $
NotFoundFF% -
(FF- .
)FF. /
;FF/ 0
returnHH 
OkHH 
(HH 
MapToDTOHH 
(HH 
postagemHH #
)HH# $
)HH$ %
;HH% &
}II 
[LL 
HttpGetLL 
(LL 
$strLL 
)LL 
]LL 
publicMM 

asyncMM 
TaskMM 
<MM 
IActionResultMM #
>MM# $
GetByFiltroMM% 0
(MM0 1
[NN 	
	FromQueryNN	 
]NN 
longNN 
?NN 
autorNN 
,NN  
[OO 	
	FromQueryOO	 
]OO 
longOO 
?OO 
temaOO 
)OO 
{PP 
ifQQ 

(QQ 
autorQQ 
isQQ 
nullQQ 
&&QQ 
temaQQ !
isQQ" $
nullQQ% )
)QQ) *
returnRR 

BadRequestRR 
(RR 
$strRR J
)RRJ K
;RRK L
ifTT 

(TT 
autorTT 
isTT 
notTT 
nullTT 
)TT 
{UU 	
varVV 
porAutorVV 
=VV 
awaitVV  
_postagemServiceVV! 1
.VV1 2
GetByAutorAsyncVV2 A
(VVA B
autorVVB G
.VVG H
ValueVVH M
)VVM N
;VVN O
returnWW 
OkWW 
(WW 
porAutorWW 
.WW 
SelectWW %
(WW% &
MapToDTOWW& .
)WW. /
)WW/ 0
;WW0 1
}XX 	
varZZ 
porTemaZZ 
=ZZ 
awaitZZ 
_postagemServiceZZ ,
.ZZ, -
GetByTemaAsyncZZ- ;
(ZZ; <
temaZZ< @
!ZZ@ A
.ZZA B
ValueZZB G
)ZZG H
;ZZH I
return[[ 
Ok[[ 
([[ 
porTema[[ 
.[[ 
Select[[  
([[  !
MapToDTO[[! )
)[[) *
)[[* +
;[[+ ,
}\\ 
[__ 
HttpPost__ 
]__ 
public`` 

async`` 
Task`` 
<`` 
IActionResult`` #
>``# $
Create``% +
(``+ ,
[``, -
FromBody``- 5
]``5 6
PostagemRequestDTO``7 I
dto``J M
)``M N
{aa 
ifbb 

(bb 
!bb 

ModelStatebb 
.bb 
IsValidbb 
)bb  
returnbb! '

BadRequestbb( 2
(bb2 3

ModelStatebb3 =
)bb= >
;bb> ?
vardd 
postagemdd 
=dd 
newdd 
Postagemdd #
{ee 	
Tituloff 
=ff 
dtoff 
.ff 
Tituloff  
,ff  !
Textogg 
=gg 
dtogg 
.gg 
Textogg 
,gg  
Temahh 
=hh 
dtohh 
.hh 
TemaIdhh  
.hh  !
HasValuehh! )
?ii 
awaitii 
_temaServiceii  ,
.ii, -
GetByIdAsyncii- 9
(ii9 :
dtoii: =
.ii= >
TemaIdii> D
.iiD E
ValueiiE J
)iiJ K
:jj 
nulljj 
,jj 
Usuariokk 
=kk 
dtokk 
.kk 
	UsuarioIdkk #
.kk# $
HasValuekk$ ,
?ll 
awaitll 
_usuarioServicell  /
.ll/ 0
GetByIdAsyncll0 <
(ll< =
dtoll= @
.ll@ A
	UsuarioIdllA J
.llJ K
ValuellK P
)llP Q
:mm 
nullmm 
}nn 	
;nn	 

varpp 
createdpp 
=pp 
awaitpp 
_postagemServicepp ,
.pp, -
CreateAsyncpp- 8
(pp8 9
postagempp9 A
)ppA B
;ppB C
ifqq 

(qq 
createdqq 
isqq 
nullqq 
)qq 
returnqq #

BadRequestqq$ .
(qq. /
)qq/ 0
;qq0 1
returnss 
CreatedAtActionss 
(ss 
nameofss %
(ss% &
GetByIdss& -
)ss- .
,ss. /
newss0 3
{ss4 5
idss6 8
=ss9 :
createdss; B
.ssB C
IdssC E
}ssF G
,ssG H
MapToDTOssI Q
(ssQ R
createdssR Y
)ssY Z
)ssZ [
;ss[ \
}tt 
[ww 
HttpPutww 
(ww 
$strww 
)ww 
]ww 
publicxx 

asyncxx 
Taskxx 
<xx 
IActionResultxx #
>xx# $
Updatexx% +
(xx+ ,
longxx, 0
idxx1 3
,xx3 4
[xx5 6
FromBodyxx6 >
]xx> ?
PostagemRequestDTOxx@ R
dtoxxS V
)xxV W
{yy 
ifzz 

(zz 
!zz 

ModelStatezz 
.zz 
IsValidzz 
)zz  
returnzz! '

BadRequestzz( 2
(zz2 3

ModelStatezz3 =
)zz= >
;zz> ?
var|| 
postagem|| 
=|| 
new|| 
Postagem|| #
{}} 	
Id~~ 
=~~ 
id~~ 
,~~ 
Titulo 
= 
dto 
. 
Titulo  
,  !
Texto
ÄÄ 
=
ÄÄ 
dto
ÄÄ 
.
ÄÄ 
Texto
ÄÄ 
,
ÄÄ  
Tema
ÅÅ 
=
ÅÅ 
dto
ÅÅ 
.
ÅÅ 
TemaId
ÅÅ  
.
ÅÅ  !
HasValue
ÅÅ! )
?
ÇÇ 
await
ÇÇ 
_temaService
ÇÇ  ,
.
ÇÇ, -
GetByIdAsync
ÇÇ- 9
(
ÇÇ9 :
dto
ÇÇ: =
.
ÇÇ= >
TemaId
ÇÇ> D
.
ÇÇD E
Value
ÇÇE J
)
ÇÇJ K
:
ÉÉ 
null
ÉÉ 
,
ÉÉ 
Usuario
ÑÑ 
=
ÑÑ 
dto
ÑÑ 
.
ÑÑ 
	UsuarioId
ÑÑ #
.
ÑÑ# $
HasValue
ÑÑ$ ,
?
ÖÖ 
await
ÖÖ 
_usuarioService
ÖÖ  /
.
ÖÖ/ 0
GetByIdAsync
ÖÖ0 <
(
ÖÖ< =
dto
ÖÖ= @
.
ÖÖ@ A
	UsuarioId
ÖÖA J
.
ÖÖJ K
Value
ÖÖK P
)
ÖÖP Q
:
ÜÜ 
null
ÜÜ 
}
áá 	
;
áá	 

var
ââ 
updated
ââ 
=
ââ 
await
ââ 
_postagemService
ââ ,
.
ââ, -
UpdateAsync
ââ- 8
(
ââ8 9
postagem
ââ9 A
)
ââA B
;
ââB C
if
ää 

(
ää 
updated
ää 
is
ää 
null
ää 
)
ää 
return
ää #
NotFound
ää$ ,
(
ää, -
)
ää- .
;
ää. /
return
åå 
Ok
åå 
(
åå 
MapToDTO
åå 
(
åå 
updated
åå "
)
åå" #
)
åå# $
;
åå$ %
}
çç 
[
êê 

HttpDelete
êê 
(
êê 
$str
êê 
)
êê 
]
êê 
public
ëë 

async
ëë 
Task
ëë 
<
ëë 
IActionResult
ëë #
>
ëë# $
Delete
ëë% +
(
ëë+ ,
long
ëë, 0
id
ëë1 3
)
ëë3 4
{
íí 
var
ìì 
deleted
ìì 
=
ìì 
await
ìì 
_postagemService
ìì ,
.
ìì, -
DeleteAsync
ìì- 8
(
ìì8 9
id
ìì9 ;
)
ìì; <
;
ìì< =
if
îî 

(
îî 
!
îî 
deleted
îî 
)
îî 
return
îî 
NotFound
îî %
(
îî% &
)
îî& '
;
îî' (
return
ïï 
	NoContent
ïï 
(
ïï 
)
ïï 
;
ïï 
}
ññ 
}óó ‡
wC:\Users\mauri\OneDrive\Documentos\GitHub\Acelera-Maker\Projeto Blog Pessoal\BlogPessoal\Controllers\IA\IAController.cs
	namespace		 	
BlogPessoal		
 
.		 
Controllers		 !
.		! "
IA		" $
;		$ %
[ 
	Authorize 

]
 
[ 
ApiController 
] 
[ 
Route 
( 
$str 
) 
] 
public 
class 
IAController 
: 
ControllerBase *
{ 
private 
readonly 

IIAService 

_iaService  *
;* +
public 

IAController 
( 

IIAService "
	iaService# ,
), -
{ 

_iaService 
= 
	iaService 
; 
} 
[ 
HttpPost 
( 
$str 
) 
] 
public 

async 
Task 
< 
IActionResult #
># $
Resumir% ,
(, -
[- .
FromBody. 6
]6 7
string8 >
texto? D
)D E
{ 
if 

( 
string 
. 
IsNullOrWhiteSpace %
(% &
texto& +
)+ ,
), -
return 

BadRequest 
( 
$str ;
); <
;< =
var"" 
	resultado"" 
="" 
await"" 

_iaService"" (
.""( )
GerarResumoAsync"") 9
(""9 :
texto"": ?
)""? @
;""@ A
return%% 
Ok%% 
(%% 
	resultado%% 
)%% 
;%% 
}&& 
}'' 