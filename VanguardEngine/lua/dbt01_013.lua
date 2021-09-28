-- Dragon Deity King of Resurgence, Dragveda

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Other, o.Resting, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOverTrigger, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastRevealedTrigger() and obj.Exists(1) then
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
		obj.ChooseStand(1)
	end
	return 0
end