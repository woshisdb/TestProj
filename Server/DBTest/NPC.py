#表示需求的基类
class Need:
    def __init__(self, rate):
        """
        初始化需求对象
        :param rate: 需求所占的比例
        """
        self.rate = rate
    #对食物的评级
    def getFoodRate(self):
        return {
            'quality': 0,
            'trait': 0,
            'price': 0,
            'brand':0,
        }
    def getHouseRate(self):
        return {
            'quality': 0,
            'quality': 0,
            'quality': 0,
            'quality': 0,
        }
#对活着的需求
class Alive(Need):
    def __init__(self, rate):
        super().__init__(rate)
    def getFoodRate(self):
        return {
            'brand':0,
            'quality': 0,
            'trait': 0,
            'price': -1,#越小越好
        }
#对安全的需求
class Safety(Need):
    def __init__(self, rate):
        super().__init__(rate)
    def getFoodRate(self):
        return {
            'brand':0,
            'quality': 0,
            'trait': 0,
            'price': -1,#约小越好
        }
#对享乐的需求
class Pleasure(Need):
    def __init__(self, rate):
        super().__init__(rate)
    def getFoodRate(self):
        return {
            'brand':0,
            'quality': 0,
            'trait': 1,
            'price': 0,
        }

#对归属的需求
class Belong(Need):
    def __init__(self, rate):
        super().__init__(rate)
    def getFoodRate(self):
        return {
            'brand':1,
            'quality': 0,
            'trait': 0,
            'price': 0,
        }
#对尊重的需求
class Esteem(Need):
    def __init__(self, rate):
        super().__init__(rate)
    def getFoodRate(self):
        return {
            'brand':0,
            'quality': 0,
            'trait': 0,
            'price': 0,
        }

class NPC:
    def __init__(self):
        #########################################
        self.alive = Alive(rate=0.8)
        self.safety = Safety(rate=0.9)
        self.belong = Belong(rate=0.7)
        self.esteem = Esteem(rate=0.6)
