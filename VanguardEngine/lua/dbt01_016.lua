-- Hades Deity King of Resentment, Gallmageheld

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOverTrigger, t.OverTrigger, p.HasPrompt, true, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.OnTriggerZone() then
			return true
		end
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.AddCirclePower(FL.PlayerVanguard, 10000)
		obj.AddCircleCritical(FL.PlayerVanguard, 1)
	end
	return 0
end