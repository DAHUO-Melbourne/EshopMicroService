104. 修改launchSettings.json文件来修改configuration
105. basket主要是有两个数据来源：db和redis distributed cache
107. create一个model folder，里面包含了basket的model
121. 我来说一下.net连postgrasql的原理吧：连接数据库用的就是appSettings里的connectionString的信息
	 但是如何根据connectionString里的信息启动对饮数据库呢？你需要根据connectionString里的数值编写对应的docker file，从而使用docker-compose来运行启动数据库的指令
126. 当你的docker遇到问题：
	
	docker-compose -f docker-compose.yml -f docker-compose.override.yml down

	docker volume rm <your_project_name>_postgres_catalog
	docker volume rm <your_project_name>_postgres_basket

	docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build -d

	docker ps

	docker logs basketdb

127. 我们将会添加redis作为cache工具，来减少我们与db之间不必要的交互；这是一个key-value store，用于cache/session storage/pub-sub systems
128. 讲了一下cache的原理: 
	 首先：client 查找数据的时候，先查找一下Cache的数据；如果cache里有数据，就用cache里的数据respond to client
	 但是如果cache里没有这一数据，则从db里查找数据，存储在cache中，然后再返回给client