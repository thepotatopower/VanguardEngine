-- 堅鋭竜 ゲイツフォート

function RegisterAbilities()
	-- on retire
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRetire)
	ability1.SetLocation(l.BackRowRC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsPlayerTurn() and obj.Exists({q.Location, l.Applicable, q.Other, o.Enemy})
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.Stand({q.Other, o.ThisFieldID})
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 2000, p.UntilEndOfTurn)
end