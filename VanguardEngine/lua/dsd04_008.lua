-- 友情の騎士 サイラス

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function Activation()
	obj.RevealFromDeck(1)
	obj.SuperiorCall({q.Location, l.Revealed, q.Other, o.Unit})
	obj.AddToHand({q.Location, l.Revealed})
end