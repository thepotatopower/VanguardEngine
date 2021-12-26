-- 樹角獣 ヘジック

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player})
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end