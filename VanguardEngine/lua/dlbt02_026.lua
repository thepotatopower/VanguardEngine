-- 目指すは頂上 カトレイン

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanDiscard(2) end
	obj.CounterBlast(1)
	obj.Discard(2)
end

function Activation()
	obj.Draw(2)
end