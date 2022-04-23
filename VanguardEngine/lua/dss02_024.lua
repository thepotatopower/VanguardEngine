-- 極光戦姫 ホーン・アプリコット

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster() and obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Seraph"), q.Other, o.Attacking})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.HasPrison() and obj.Exists({q.Location, l.EnemyDrop, q.UnitType, u.Trigger})
end

function Activation()
	obj.EnemyChooseImprison({q.Location, l.EnemyDrop, q.UnitType, u.Trigger, q.Count, 1})
end