-- 砂塵の曲射 アラスター

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
	return not obj.Exists({q.Location, l.EnemyUnits, q.Other, o.Boosting})
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusShield, 10000, p.UntilEndOfBattle)
end