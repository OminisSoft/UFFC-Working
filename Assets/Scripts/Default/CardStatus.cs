namespace Default {
    
    /**
     * Objeto responsável por armazenar os
     * detalhes de cada template de carta
     * do jogo
     */
    public class CardStatus {

        private readonly bool _isTerrain;
        private readonly bool _isSpecial;
        private readonly int[] _manaGen;
        private readonly int[] _manaCost;
        private readonly int _cardPower;
        private readonly int _cardDefense;
        private readonly int _maxCopies;

        public CardStatus(bool isTerrain, bool isSpecial, int[] manaGen, int[] manaCost, int cardPower, int cardDefense, int maxCopies) {
            _isTerrain = isTerrain;
            _isSpecial = isSpecial;
            _manaGen = manaGen;
            _manaCost = manaCost;
            _cardPower = cardPower;
            _cardDefense = cardDefense;
            _maxCopies = maxCopies;
        }
        
        public bool isTerrain => _isTerrain;
        public bool isSpecial => _isSpecial;
        public int[] manaGen => _manaGen;
        public int[] manaCost => _manaCost;
        public int cardPower => _cardPower;
        public int cardDefense => _cardDefense;
        public int maxCopies => _maxCopies;

    }
}