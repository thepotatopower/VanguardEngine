-- ディフレクトバルス・ドラゴン

function RegisterAbilities()
	-- on put
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PutOnGC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerRC, q.Grade, 0})
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusShield, 5000, p.UntilEndOfBattle)
end