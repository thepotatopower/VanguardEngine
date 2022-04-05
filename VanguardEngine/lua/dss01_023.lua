-- 極光戦姫 スパーク・リモーネ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() 
end

function Condition()
	return obj.HasPrison() and obj.Exists({q.Location, l.EnemyDrop, q.Other, o.Unit})
end

function Activation()
	obj.ChooseImprison({q.Location, l.EnemyDrop, q.Other, o.Unit, q.Count, 2})
end