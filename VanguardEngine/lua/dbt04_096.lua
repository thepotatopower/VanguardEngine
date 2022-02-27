-- 極光戦姫捜査網 激録大追跡！

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function CanFullyResolve()
	return obj.HasPrison() and obj.Exists({q.Location, l.BackRowEnemyRC})
end

function Activation()
	obj.Imprison({q.Location, l.BackRowEnemyRC})
end