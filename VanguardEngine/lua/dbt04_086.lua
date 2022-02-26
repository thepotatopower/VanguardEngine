-- 極光戦姫 チェイシング・ネール

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.HasPrison() and obj.Exists({q.Location, l.EnemyHand, q.Count, 1})
end

function Activation()
	if obj.HasPrison() then
		obj.EnemyChooseImprison({q.Location, l.EnemyHand, q.Count, 1})
	end
end