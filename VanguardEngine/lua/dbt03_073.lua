-- ピザーシング・アシスタント

function RegisterAbilities()
	-- on soul charge
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnSoulCharge)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.SourceIsPlayerAbility()
end

function Condition()
	return obj.PersonaRode()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.Draw(1)
end