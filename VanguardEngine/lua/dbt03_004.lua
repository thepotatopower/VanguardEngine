-- ディアブロスリターナー デーリック

function RegisterAbilities()
	-- continuous
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.PlayerRC)
	ability1.SetActivation("Cont")
	-- ACT
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnACT)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetCost("Cost")
	ability2.SetActivation("Activation")
end

function Cont()
	if obj.InFinalRush() then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanAddToSoul({q.Location, l.PlayerRC, q.Count, 1}) end
	obj.CounterBlast(1)
	obj.ChooseAddToSoul({q.Location, l.PlayerRC, q.Count, 1})
end

function Activation()
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, math.floor(obj.SoulCount() / 4)})
	if obj.InFinalRush() then
		obj.LookAtTopOfDeck(obj.NumEnemyOpenCircles())
		obj.SuperiorCall({q.Location, l.Looking, q.Min, 0})
		obj.AddToSoul({q.Location, l.Looking})
	end
end