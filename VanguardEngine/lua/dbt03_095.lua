-- 驚嘆の天核 フォルティード

function RegisterAbilities()
	-- on stand
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnStand)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player})
end

function Cost(check)
	if check then return obj.CanRest({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}) end
	obj.Rest({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.NotThis})
end

function Activation()
	obj.Select({q.Location, l.PlayerRC, q.Other, o.NotThis})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end