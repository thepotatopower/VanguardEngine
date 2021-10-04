-- Fantastic Fur-nale, Catrina

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 0
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOverTrigger, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastRevealedTrigger() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddPermanentPlayerState(ps.RearguardPower10000)
	end
	return 0
end