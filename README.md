# Neural-Networks
example of neural networks/genetic algorithms in Unity3D

Esse é um exemplo de redes neurais com algoritimos genéticos

eu segui o tutorial em c++ no http://www.ai-junkie.com/

o código está em c# e em português usando o unity3D como interface,

Existem os tanques que tem que coletar os pontos verdes espalhados no chão,

quando pegam os pontos verdes, os pontos desaparecem e reaperece num lugar aleatório

Existem 4 coisas(4 números) que o tanque vê e que será o input pra rede neural: 

dois deles representam o vetor apontando pro ponto verde mais perto

e os outros dois representam a direção que o tanque está olhando

assim, dados esses inputs o tanque tem que desvendar como transformar eles para poder pegar o ponto

o output da rede neural são dois números:
o quanto que ele pode virar pra esquerda ou pra direita,

esses dois números vão ditar sua rotação,

os pesos das redes são atualizados na interação (algoritimo genético)

e são modificados de acordo com um fitness apropriado

os mais qualificados "sobrevivem" e fazem crossover com os demais

para que todos fiquem com a melhor estratégia

bem, depois de umas 50 gerações eles ficam razoavelmente bons em achar o ponto

boa sorte, caso o código esteja muito complicado pra entender a idéia geral

mande um email: wender_jipa0@hotmail.com


This is a example of neural networks plus genetic algorithms, 

i followed this tutorial in c++ http://www.ai-junkie.com/ (this site is awesome)

my code is in C# with Unity3D as interface (All code is in Portuguese!)

there are tanks that have to find and collect the green point scattered about a simple 2D world, 

there is four inputs, two of them represent a vector pointing to the closest green point 

and the other two represent the direction the tank is pointing. 

These four inputs give the tank's brain - its neural network

 everything it needs to know to figure out how to orient itself towards the green points 

(tanks can control how to rotate) aand after 50 generations they can find the green points very fast.

hope you enjoy :D
