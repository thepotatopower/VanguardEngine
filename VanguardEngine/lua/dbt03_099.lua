-- 天弾の騎士 プロクリス

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerRC, q.Other, o.This}) and obj.CanRest({q.Location, l.PlayerRC, q.Other, o.NotThis}) end
	obj.AddToSoul({q.Location, l.PlayerRC, q.Other, o.This})
	obj.ChooseRest({q.Location, l.PlayerRC, q.Other, o.NotThis})
end

function Activation()
	obj.LookAtTopOfDeck(3)
	local added = obj.ChooseAddToHand({q.Location, l.Looking, q.Grade, 3, q.Min, 0})
	obj.Store(added)
	obj.Reveal({q.Location, l.Stored})
	obj.AddToDrop({q.Location, l.Looking})
end