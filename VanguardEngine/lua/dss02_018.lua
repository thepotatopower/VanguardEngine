-- デザイアデビル シットー

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnPut)
	ability1.SetMovedTo(l.Soul)
	ability1.SetMovedFrom(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.SourceIsVanguardAbility() and obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.Drop})
end

function Activation()
	obj.ChooseAddToSoul({q.Location, l.Drop, q.Count, 1})
end