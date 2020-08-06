Proje Kapsamı

İki uç nokta arasında doğrudan canlı ses, video (webcam veya ekran gürüntüsü) ve media paylaşımı için geliştirilmiştir.

Genel Açıklama 

Projede iki eş arasında doğrudan bağlantıyı sağlamak için WebRTC teknolojisi kullanılmıştır. WebRTC açık kaynak kod uygulamarından oldukça yararlanmış olup  (https://github.com/webrtc-uwp/webrtc-uwp-sdk) üzerine wrapper tarzında geliştirilmiştir.

Ugulama İçi Görüntüler

J.	PROJECT DESIGN
1.	Architectural Design
i.	System Architecture
 



2.	Application Design
i.	Operations Page
This page is the main page of app it provides functionality such as closing or opening of streams, changing of stream sources and their configurations also starting of call 
In the start, Application connect to signaling server thus it gives her a unique Id. It is used for starting call.
 
User can close or opens stream to clicks on bottom buttons and can close loopback video to clicks bottom right button. In this sample video and audio streams are closed but video loopback is open.   
ii.	To Change Streams Settings
iii.	User must click on settings button which is represented as wheel. And  a settings panel is opened up.
 
User can change sources for stream and also he can change speaker. Capture resolution is related to source device. Application only shows supported profile for device.
After user is configured the settings. Application saves them into application settings thus another time it recognizes configurations.
iv.	To Close or Open Streams
To realize it. User must click on video button (which is represented as camera image) or microphone button (which is represented as mic image) to close or open streams.
In this sample video and mic is closed and video loop back is open. Closed stream or loop back is represented like its background red and a slash image lays cross over it. 
 
v.	To Call to Peer
To realize it. After all user must type a unique id which is owned by a remote peer which has already connected to same server with user.
Id was given by signaling server to a peer. In addition, signaling server gives all peers, which are connected to self, information.
In this sample remote Id of remote peer is 2. After typing the Id. User must click on connect button which is represented by on of image. 
If typed file is existing in between Id of peers. Application starts connection and navigate to Remote Connection Page. In this page all connection operations can be perform. In opposite case application will give a mismatching error.
 
3.	Remote Connection Page
vi.	To Select Shearing Screen
After starting call, at the beginning of the connection for screen shearing, user must select a window to share or he can cancel the sharing. 
 
vii.	To Send Message and File
One-time connection is established. User can make file offer or send message to the peer. In this sample, one message and a file offer message were sent to peer. 
For sending file, user must click on button with attach icon and must select a file or files form opened panel. After user selects file and confirm it. As to be async, all files are sent to the peer as message.
For message sending, firstly user must type a text into text box and then must clicks on send button which is represented as left to right arrow.
 
viii.	To Accept a File Offer
User must click on received message on message box and later  he can select a folder and name for file or can cancel it.
If user confirm, application send to remote peer system a message like “If you are idle , can you send it to me“ this means if connection has already a running Task (A downloading or uploading event) it add it to standby to run on later or not it start sending after a series of logical sate notification (This is not the concern of end user). 
Tasks are processed as first in first out logic (FIFO), of course user can cancel either running or waiting or downloading or uploading Task. 
In the below demonstration, As notified, user can understand a running Task is represented with yellow and waiting with blue.
 
Here, as can see, ended Task is represented with green color.
 
ix.	To Open File or Its Folder
In receiver side, after download has ended with success. He can clicks on button with OK image to open file or can click on button with folder icon to open file folder.
In sender side, he can open up folder without waiting of Task end but for to open file, he needs to wait for end of Task.
In this sample, receiver had downloaded a 7z file and he was opened it up.
 
x.	To Close Call
For to close call. User just click on close button in top bar to close connection. After click, app close peer connection and give a notify the users both side.
To return back user must click on bottom left button to turn the Operations Page.
 


![alt text](https://icdn.ensonhaber.com/resimler/galeri/1_4813.jpg)
