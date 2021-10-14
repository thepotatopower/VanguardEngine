-- Stealth Dragon, Tensha Stead

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC
	elseif n == 4 then
		return q.Location, l.Deck, q.Name, "", q.Count, 1
	elseif n == 5 then
		return q.Location, l.Selected
	end
end                                     

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHits, p.HasPrompt, p.CB, 1, p.Retire, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsBooster() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.ExistsAmong(3, 4, q.Name) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Select(2)
		obj.Inject(4, q.Name, obj.GetSelectedName(5))
		obj.Search(4)
		obj.Shuffle()
	end
	return 0
end